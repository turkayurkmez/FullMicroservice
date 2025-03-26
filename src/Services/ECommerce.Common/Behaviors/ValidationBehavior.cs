using ECommerce.Common.Results;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Common.Behaviours
{

    // ValidationBehavior sınıfı, istekleri doğrulamak için bir ara katman (pipeline) davranışıdır.
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> _validators, ILogger<ValidationBehavior<TRequest, TResponse>> _logger) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse> // TRequest, IRequest<TResponse> arayüzünü uygulamalıdır.
        where TResponse : class // TResponse, bir sınıf olmalıdır.
    {
        // Handle metodu, isteği işlemek için doğrulama yapar.
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Eğer doğrulayıcı yoksa, bir sonraki işleyiciye geç.
            if (!_validators.Any())
            {
                return await next();
            }

            // İstek için doğrulama bağlamı oluştur....
            var context = new ValidationContext<TRequest>(request);

            // Tüm doğrulayıcıları asenkron olarak çalıştır ve sonuçları al.
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Tüm doğrulama hatalarını topla.
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            // Eğer hata yoksa, bir sonraki işleyiciye geç.
            if (failures.Count == 0)
            {
                return await next();
            }

            // Hata durumunda, log kaydı oluştur.
            _logger.LogWarning($"{typeof(TRequest).Name} isteği için doğrulama hatası oluştu. Detaylar: {string.Join(", ", failures.Select(f => f.ErrorMessage))}");

            // Eğer TResponse, Result<T> türündeyse, hata döndür.
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                // Result<T> türündeki hata metodu al.
                var resultType = typeof(Result<>).GetGenericArguments()[0];
                // Failure metodu oluştur.
                var failureMethod = typeof(Result<>).MakeGenericType(resultType).GetMethod("Failure", new Type[] { typeof(IEnumerable<string>) });

                // Eğer hata metodu varsa, hata döndür.
                if (failureMethod != null)
                {
                    // Hataları oluştur.
                    var errors = failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}").ToList();
                    // Hata metodu çağır ve sonucu döndür.
                    return (TResponse)failureMethod.Invoke(null, new object[] { errors })!;
                }
            }

            // Eğer TResponse, Result türündeyse, hata döndür.
            if (typeof(TResponse) == typeof(Result))
            {
                // Hataları oluştur.
                var errors = failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}").ToList();
                // Hata döndür.
                return Result.Failure(errors) as TResponse ?? throw new InvalidOperationException("Result TResponse'a dönüştürülemiyor.");
            }

            // Eğer hata varsa, ValidationException fırlat.
            throw new ValidationException(failures);
        }
    }
}

using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {

        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation($"{requestName} isteği alındı.");

            try
            {
                var response = await next();

                _logger.LogInformation($"{requestName} isteği başarıyla işlendi.");

                return response;
            }
            catch (Exception)
            {
                _logger.LogError($"{requestName} isteği sırasında bir hata oluştu.");
                throw;
            }
        }
    }
}

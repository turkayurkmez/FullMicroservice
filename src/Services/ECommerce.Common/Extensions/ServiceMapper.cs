using Mapster;
using MapsterMapper;

namespace ECommerce.Common.Extensions
{
    using System;
    // Sistem sağlayıcısını kullanarak haritalama işlemleri gerçekleştiren sınıf
    public class ServiceMapper : IMapper
    {
        private readonly IServiceProvider _serviceProvider; // Hizmet sağlayıcısı

        // Global ayarları içeren yapılandırma
        public TypeAdapterConfig Config { get; } = TypeAdapterConfig.GlobalSettings;

        // ServiceMapper sınıfının yapıcı metodu
        public ServiceMapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider; // Sağlayıcıyı ata
        }

        // Kaynaktan hedefe haritalama işlemi
        public TDestination Adapt<TSource, TDestination>(TSource source)
        {
            return source.Adapt<TDestination>(Config); // Kaynağı hedefe dönüştür
        }

        // Kaynaktan hedefe haritalama işlemi (object türü)
        public TDestination Adapt<TDestination>(object source)
        {
            return source.Adapt<TDestination>(Config); // Kaynağı hedefe dönüştür
        }

        // Kaynaktan hedefe haritalama işlemi (var olan hedef ile)
        public TDestination Adapt<TSource, TDestination>(TSource source, TDestination destination)
        {
            return source.Adapt(destination, Config); // Kaynağı mevcut hedefe dönüştür
        }

        // Kaynağı belirtilen türlere göre haritalama işlemi
        public object Adapt(object source, Type sourceType, Type destinationType)
        {
            return source.Adapt(destinationType, Config); // Kaynağı hedef türüne dönüştür
        }

        // Kaynağı belirtilen türlere göre mevcut hedef ile haritalama işlemi
        public object Adapt(object source, object destination, Type sourceType, Type destinationType)
        {
            return source.Adapt(destination, sourceType, destinationType, Config); // Kaynağı mevcut hedefe dönüştür
        }

        // Belirtilen kaynaktan bir adaptör oluşturma
        public ITypeAdapterBuilder<TSource> From<TSource>(TSource source)
        {
            return source.BuildAdapter(Config); // Adaptör oluştur
        }

        // Kaynaktan hedefe haritalama işlemi (object türü)
        public TDestination Map<TDestination>(object source)
        {
            return Adapt<TDestination>(source); // Kaynağı hedefe dönüştür
        }

        // Kaynaktan hedefe haritalama işlemi
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Adapt<TSource, TDestination>(source); // Kaynağı hedefe dönüştür
        }

        // Kaynaktan mevcut hedefe haritalama işlemi
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return Adapt(source, destination); // Kaynağı mevcut hedefe dönüştür
        }

        // Kaynağı belirtilen türlere göre haritalama işlemi (object türü)
        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Adapt(source, sourceType, destinationType); // Kaynağı hedef türüne dönüştür
        }

        // Kaynağı belirtilen türlere göre mevcut hedef ile haritalama işlemi
        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return Adapt(source, destination, sourceType, destinationType); // Kaynağı mevcut hedefe dönüştür
        }
    }
}
using AutoMapper;

namespace API.Helpers
{
    public class PurchaseFormatter : IValueConverter<double, decimal>
    {
        public decimal Convert(double source, ResolutionContext context) => (decimal)source;
    }
}
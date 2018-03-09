using Data.Contracts;
using Data.Persistance;

namespace Data.Services
{
    public static class UnitOfWork
    {
        public static bool Test { get; set; }

        public static IUnitOfWork Start(bool? test = null)
        {
            var connection = Connections.Get(test ?? Test);
            return new Persistance.UnitOfWork(() => new Context(connection));
        }
    }
}
using Soap.Infrastructure.Entities;
using Soap.Models;
using Soap.Contracts;

namespace Soap.Mappers
{
    public static class StatsMapper
    {
        public static Dtos.Stats ToDto(this Soap.Models.Stats modelStats)
        {
            if (modelStats == null)
            {
                return null;
            }

            return new Dtos.Stats
            {
                St = modelStats.St,
                Ma = modelStats.Ma,
                En = modelStats.En,
                Ag = modelStats.Ag,
                Lu = modelStats.Lu
            };
        }
        public static Soap.Models.Stats ToModel(this Soap.Infrastructure.Entities.Stats entityStats)
        {
            if (entityStats == null)
            {
                return null;
            }

            return new Soap.Models.Stats
            {
                St = entityStats.St,
                Ma = entityStats.Ma,
                En = entityStats.En,
                Ag = entityStats.Ag,
                Lu = entityStats.Lu
            };
        }
        public static Dtos.Stats ToDto(this Infrastructure.Entities.Stats entityStats)
        {
            if (entityStats == null)
            {
                return null;
            }

            return new Dtos.Stats
            {
                St = entityStats.St,
                Ma = entityStats.Ma,
                En = entityStats.En,
                Ag = entityStats.Ag,
                Lu = entityStats.Lu
            };
        }
    }
}

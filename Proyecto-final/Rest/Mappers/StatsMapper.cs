using Rest.Infraestructure.Entities;
using Rest.Models;
using Rest.Controllers;
using Rest.Dtos;

namespace Rest.Mappers
{
    public static class StatsMapper
    {
        public static Dtos.Stats ToDto(this Rest.Models.Stats modelStats)
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
        public static Rest.Models.Stats ToModel(this Rest.Infraestructure.Entities.PerEntity.Stats entityStats)
        {
            if (entityStats == null)
            {
                return null;
            }

            return new Rest.Models.Stats
            {
                St = entityStats.St,
                Ma = entityStats.Ma,
                En = entityStats.En,
                Ag = entityStats.Ag,
                Lu = entityStats.Lu
            };
        }
        public static Dtos.Stats ToDto(this Infraestructure.Entities.PerEntity.Stats entityStats)
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

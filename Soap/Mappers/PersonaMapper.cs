using Soap.Dtos;
using Soap.Models;
using Soap.Infrastructure.Entities;
using System.Collections.Generic;

namespace Soap.Mappers
{
    public static class PersonaMapper
    {
        public static PersonaModel ToModel(this PersonaEntity persona)
        {
            if (persona is null)
            {
                return null;
            }
            return new PersonaModel
            {
                Arcana = persona.Arcana,
                Weak = new List<string>(persona.Weak),
                Stats = persona.Stats != null ? persona.Stats.ConvertAll(s => new Stats
                {
                    St = s.St,
                    Ma = s.Ma,
                    En = s.En,
                    Ag = s.Ag,
                    Lu = s.Lu
                }) : new List<Stats>(),
                Strength = new List<string>(persona.Strength),
                Level = persona.Level,
                Name = persona.Name
            };
        }

        public static PersonaResponseDto ToDto(this PersonaModel persona)
        {
            return new PersonaResponseDto
            {
                Name = persona.Name,
                Arcana = persona.Arcana,
                Weak = persona.Weak,
                Strength = persona.Strength,
                Level = persona.Level,
                Stats = persona.Stats != null ? persona.Stats.ConvertAll(s => new StatsDto
                {
                    St = s.St,
                    Ma = s.Ma,
                    En = s.En,
                    Ag = s.Ag,
                    Lu = s.Lu
                }) : new List<StatsDto>()
            };
        }

        public static PersonaEntity ToEntity(this PersonaModel persona)
        {
            return new PersonaEntity
            {
                Arcana = persona.Arcana,
                Weak = new List<string>(persona.Weak),
                Stats = persona.Stats != null ? persona.Stats.ConvertAll(s => new StatsEntity
                {
                    St = s.St,
                    Ma = s.Ma,
                    En = s.En,
                    Ag = s.Ag,
                    Lu = s.Lu
                }) : new List<StatsEntity>(),
                Strength = new List<string>(persona.Strength),
                Level = persona.Level,
                Name = persona.Name
            };
        }

        public static PersonaModel ToModel(this PersonaCreateRequestDto persona)
        {
            return new PersonaModel
            {
                Arcana = persona.Arcana,
                Weak = new List<string>(persona.Weak),
                Stats = persona.Stats != null ? persona.Stats.ConvertAll(s => new Stats
                {
                    St = s.St,
                    Ma = s.Ma,
                    En = s.En,
                    Ag = s.Ag,
                    Lu = s.Lu
                }) : new List<Stats>(),
                Strength = new List<string>(persona.Strength),
                Level = persona.Level,
                Name = persona.Name
            };
        }

        public static PersonaModel ToModel(this PersonaUpdateDto persona)
        {
            return new PersonaModel
            {
                Arcana = persona.Arcana,
                Weak = persona.Weak,
                Strength = persona.Strength,
                Level = persona.Level,
                Name = persona.Name,
                Stats = persona.Stats != null ? persona.Stats.ConvertAll(s => new Stats
                {
                    St = s.St,
                    Ma = s.Ma,
                    En = s.En,
                    Ag = s.Ag,
                    Lu = s.Lu
                }) : new List<Stats>()
            };
        }
    }
}

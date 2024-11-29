using Soap.Dtos;
using Soap.Infrastructure.Entities;
using Soap.Models;
using MongoDB.Bson;

namespace Soap.Mappers
{
    public static class PerMapper
    {
        public static PerModel ToModel(this PerEntity persona)
        {
            if (persona == null)
            {
                return null;
            }

            return new PerModel
            {
                id = persona.id,
                name = persona.name,
                arcana = persona.arcana,
                level = persona.level,
                stats = persona.stats?.ToModel(),  // Convertir de Stats de Entity a Stats de Model
                strength = persona.strength,
                weak = persona.weak
            };
        }

        public static PerResponseDto ToDto(this PerModel persona)
        {
            return new PerResponseDto
            {
                id = persona.id,
                name = persona.name,
                arcana = persona.arcana,
                level = persona.level,
                stats = persona.stats?.ToDto(),  // Stats ya es del tipo correcto
                strength = persona.strength,
                weak = persona.weak
            };
        }
        public static PerResponseDto ToDto(this PerEntity persona)
        {
            return new PerResponseDto
            {
                id = persona.id,
                name = persona.name,
                arcana = persona.arcana,
                level = persona.level,
                stats = persona.stats?.ToDto(),
                strength = persona.strength,
                weak = persona.weak
            };
        }

        public static Infrastructure.Entities.PerEntity toEntity(this Dtos.PerResponseDto dto)
    {
        if (dto == null)
        {
            return null;
        }

        // Verifica si dto.stats es nulo antes de acceder a sus propiedades.
        Infrastructure.Entities.Stats entityStats = dto.stats != null ? new Infrastructure.Entities.Stats
        {
            St = dto.stats.St,
            Ma = dto.stats.Ma,
            En = dto.stats.En,
            Ag = dto.stats.Ag,
            Lu = dto.stats.Lu
        } : null;

        return new Infrastructure.Entities.PerEntity
        {
            id = dto.id,
            name = dto.name,
            arcana = dto.arcana,
            level = dto.level,
            stats = entityStats,
            strength = dto.strength,
            weak = dto.weak
        };
    }

        // Mapea IList<PerModel> a IList<PerResponseDto>
        public static IList<PerResponseDto> ToDtoList(this IList<PerModel> personas)
        {
            if (personas == null || !personas.Any())
            {
                return new List<PerResponseDto>();
            }

            return personas.Select(persona => persona.ToDto()).ToList();
        }

        // Mapea IList<PerEntity> a IList<PerResponseDto> (si necesitas mapear directamente de la base de datos)
        public static IList<PerResponseDto> ToDtoList(this IList<PerEntity> personas)
        {
            if (personas == null || !personas.Any())
            {
                return new List<PerResponseDto>();
            }

            return personas.Select(persona => persona.ToModel().ToDto()).ToList();
        }
    }
}

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
                Id = persona.Id,
                Name = persona.Name,
                Arcana = persona.Arcana,
                Level = persona.Level,
                Stats = persona.Stats?.ToModel(),  // Convertir de Stats de Entity a Stats de Model
                Strength = persona.Strength,
                Weak = persona.Weak
            };
        }

        public static PerResponseDto ToDto(this PerModel persona)
        {
            return new PerResponseDto
            {
                id = persona.Id,
                name = persona.Name,
                arcana = persona.Arcana,
                level = persona.Level,
                stats = persona.Stats?.ToDto(),  // Stats ya es del tipo correcto
                strength = persona.Strength,
                weak = persona.Weak
            };
        }
        public static PerResponseDto ToDto(this PerEntity persona)
        {
            return new PerResponseDto
            {
                id = persona.Id,
                name = persona.Name,
                arcana = persona.Arcana,
                level = persona.Level,
                stats = persona.Stats?.ToDto(),
                strength = persona.Strength,
                weak = persona.Weak
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

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soap.Contracts.Dtos;
using Soap.Models;
using Soap.Repositories;
using Soap.Mappers;

namespace Soap.Contracts.Services
{
    public class PersonaService : IPersonaContract
    {
        private readonly IPersonaRepository _personaRepository;

        public PersonaService(IPersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        public async Task<IList<PersonaResponseDto>> GetAll(int page, int limit, CancellationToken cancellationToken)
        {
            var personas = await _personaRepository.GetAll(page,limit,cancellationToken);
            return new PersonaResponseDto{
                Arcana = personas.Arcana,
                Weak = personas.Weak,
                Stats = personas.Stats,
                Strength = personas.Strength,
                Level = personas.Level,
                Name = personas.Name
                };
        }

        public async Task<PersonaResponseDto> GetByName(string name, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetByName(name, cancellationToken);
            if (persona is not null)
            {
                return new PersonaResponseDto{
                    Arcana = persona.Arcana,
                    Weak = persona.Weak,
                    Stats = persona.Stats,
                    Strength = persona.Strength,
                    Level = persona.Level,
                    Name = persona.Name
                };
            }
        }

        public async Task<PersonaResponseDto> GetById(string id, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetById(id, cancellationToken);
            if (persona is not null){
                return new PersonaResponseDto{
                    Arcana = persona.Arcana,
                    Weak = persona.Weak,
                    Stats = persona.Stats,
                    Strength = persona.Strength,
                    Level = persona.Level,
                    Name = persona.Name
                };
            }
        }

        public async Task<IList<PersonaResponseDto>> GetByArcana(string arcana, int page, int limit, CancellationToken cancellationToken)
        {
            var personas = await _personaRepository.GetByArcana(arcana,page,limit, cancellationToken);
            return new PersonaResponseDto{
                    Arcana = personas.arcana,
                    Weak = personas.Weak,
                    Stats = personas.Stats,
                    Strength = personas.Strength,
                    Level = personas.Level,
                    Name = personas.Name
                };
        }

        public async Task<PersonaResponseDto> CreatePersona(PersonaCreateRequestDto personaRequest, CancellationToken cancellationToken)
        {
            var persona = personaRequest.ToModel();
            var createdPersona = await _personaRepository.CreatePersona(persona,cancellationToken);
            return new PersonaResponseDto{
                    Arcana = createdPersona.Arcana,
                    Weak = createdPersona.Weak,
                    Stats = createdPersona.Stats,
                    Strength = createdPersona.Strength,
                    Level = createdPersona.Level,
                    Name = createdPersona.Name
                };
        }

        public async Task<PersonaResponseDto> UpdatePersonaPartially(string id, PersonaUpdateDto updates, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetByIdAsync(id, cancellationToken);
            if (persona is null)
            {
                throw new FaultException("Persona not found");
            }

            var updatedPersona = await _personaRepository.UpdateAsync(persona, cancellationToken);
            return new PersonaResponseDto{
                    Arcana = updatedPersona.Arcana,
                    Weak = updatedPersona.Weak,
                    Stats = updatedPersona.Stats,
                    Strength = updatedPersona.Strength,
                    Level = updatedPersona.Level,
                    Name = updatedPersona.Name
                };
        }

        public async Task<PersonaResponseDto> ReplacePersona(PersonaUpdateDto personaRequest, CancellationToken cancellationToken)
        {
            var persona = personaRequest.ToModel();
            await _personaRepository.GetByIdAsync(persona.id, cancellationToken);
            if (persona is not null){
                return await _personaRepository.UpdatePersona(persona, cancellationToken);
            }
            
                throw new FaultException("Persona not found");
        }

        public async Task<bool> DeletePersonaById(string id, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetByIdAsync(id,cancellationToken);
            if (persona is null)
            {
                throw new FaultException("Persona not found");
            }
            await _personaRepository.DeleteByIdAsync(persona, cancellationToken);
            return true;
        }
    }
}

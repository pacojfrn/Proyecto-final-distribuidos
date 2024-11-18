using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soap.Contracts;
using Contracts.Dtos;
using Soap.Models;
using Soap.Repositories;

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
            return personas.Select(persona => persona.ToDto()).ToList();
        }

        public async Task<PersonaResponseDto> GetByName(string name, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetByNameAsync(name, cancellationToken);
            if (persona is not null)
            {
                return persona.ToDto();
            }
            throw new FaultException("Persona not found");
        }

        public async Task<PersonaResponseDto> GetById(string id, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetByIdAsync(id, cancellationToken);
            if (persona is not null){
                return persona.ToDto();
            }
            throw new FaultException("Persona not found");
        }

        public async Task<IList<PersonaResponseDto>> GetByArcana(string arcana, int page, int limit, CancellationToken cancellationToken)
        {
            var personas = await _personaRepository.GetPersonaByArcana(arcana,page,limit, cancellationToken);
            return personas.Select(persona => persona.ToDto()).ToList();
        }

        public async Task<PersonaResponseDto> CreatePersona(PersonaCreateRequestDto personaRequest, CancellationToken cancellationToken)
        {
            var persona = personaRequest.ToModel();
            var createdPersona = await _personaRepository.CreateAsync(persona,cancellationToken);
            return createdPersona.ToDto();
        }

        public async Task<PersonaResponseDto> UpdatePersonaPartially(string id, PersonaUpdateDto updates, CancellationToken cancellationToken)
        {
            var persona = await _personaRepository.GetByIdAsync(id, cancellationToken);
            if (persona is null)
            {
                throw new FaultException("Persona not found");
            }

            var updatedPersona = await _personaRepository.UpdateAsync(persona, cancellationToken);
            return updatedPersona.ToDto();
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

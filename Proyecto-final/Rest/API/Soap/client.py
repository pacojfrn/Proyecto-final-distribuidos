import logging
from requests import Session
import json
from zeep import Client
from zeep.transports import Transport
from zeep.exceptions import Fault
from zeep.settings import Settings

# Configurar el logger
logging.basicConfig(level=logging.DEBUG)
logger = logging.getLogger(__name__)

# URL del servicio SOAP
WSDL_URL = 'http://personas-api-svc:8080/PerService.svc?wsdl'

def get_client():
    """
    Crea y devuelve un cliente SOAP configurado.
    
    Returns:
        zeep.Client: Cliente configurado para el servicio SOAP.
    """
    session = Session()
    transport = Transport(session=session)
    settings = Settings(strict=False)
    return Client(wsdl=WSDL_URL, transport=transport, settings=settings)

def verify_persona(persona_id):
    """
    Verifica si una persona existe a través del servicio SOAP.
    
    Args:
        persona_id (str): El ID de la persona.
    
    Returns:
        dict | None: La información de la persona si existe, o None si no existe.
    """
    try:
        client = get_client()
        logger.debug(f"Llamando al servicio SOAP para verificar persona con ID={persona_id}")
        response = client.service.GetById(persona_id)
        logger.debug("Respuesta recibida del servicio SOAP")
        return response
    except Fault as fault:
        logger.error(f"SOAP Fault: {fault}")
        return None
    except Exception as e:
        logger.error(f"Error al verificar la persona: {e}")
        return None
    
def get_persona_by_name(persona_name):
    """
    Obtiene los detalles de una persona por su nombre desde el servicio SOAP.
    
    Args:
        persona_name (str): El nombre de la persona.
    
    Returns:
        dict | None: Detalles de la persona si se encuentra, o None en caso contrario.
    """
    try:
        client = get_client()
        logger.debug(f"Llamando al servicio SOAP para obtener la persona con nombre={persona_name}")
        
        # Llamada al método del servicio SOAP
        response = client.service.GetByName(persona_name)
        
        logger.debug(f"Respuesta recibida del servicio SOAP: {response}")
        return {
            'name': response.name,
            'arcana': response.arcana,
            'level': response.level,
            'stats': {
                'St': response.stats.St,
                'Ma': response.stats.Ma,
                'En': response.stats.En,
                'Ag': response.stats.Ag,
                'Lu': response.stats.Lu
            },
            "strength": [item for item in response.strength.string] if hasattr(response.strength, 'string') else [],
            "weak": [item for item in response.weak.string] if hasattr(response.weak, 'string') else [],
        }
    except Fault as fault:
        logger.error(f"SOAP Fault al obtener persona por name: {fault}")
        return None
    except Exception as e:
        logger.error(f"Error al obtener la persona por name={persona_name}: {e}")
        return None

def delete_persona(persona_name):
    """
    Elimina una persona por su ID.
    
    Args:
        persona_id (str): El ID de la persona.
    
    Returns:
        bool: True si se eliminó correctamente, False en caso de error.
    """
    try:
        client = get_client()
        logger.debug(f"Llamando al servicio SOAP para eliminar persona con Nombre={persona_name}")
        response = client.service.DeleteByName(persona_name)
        logger.debug("Respuesta recibida del servicio SOAP")
        return response
    except Fault as fault:
        logger.error(f"SOAP Fault: {fault}")
        return False
    except Exception as e:
        logger.error(f"Error al eliminar la persona: {e}")
        return False

def create_persona(name, arcana, level, stats, strength, weak):
    """
    Llama a la API SOAP para crear una persona con los datos proporcionados.

    Args:
        name (str): Nombre de la persona.
        arcana (str): Arcana de la persona.
        level (int): Nivel de la persona.
        stats (dict): Diccionario de estadísticas.
        strength (list): Lista de fortalezas.
        weak (list): Lista de debilidades.

    Returns:
        dict: Datos de la persona creada, si la creación fue exitosa.
    """
    # Configurar la sesión y el transporte para usar requests
    session = Session()
    transport = Transport(session=session)
    settings = Settings(strict=False)

    try:
        # Crear cliente SOAP
        logger.debug("Creando cliente SOAP con el WSDL")
        client = Client('http://personas-api-svc:8080/PerService.svc?wsdl', transport=transport, settings=settings)

        # Construir el objeto PerResponseDto
        per_response_dto = {
            'name': name,
            'arcana': arcana,
            'level': level,
            'stats': {
                'St': stats.get('St', 0),
                'Ma': stats.get('Ma', 0),
                'En': stats.get('En', 0),
                'Ag': stats.get('Ag', 0),
                'Lu': stats.get('Lu', 0)
            },
            'strength': strength,
            'weak': weak
        }
       
        # Llamar al servicio SOAP para crear una persona
        logger.debug("Llamando al servicio SOAP para crear persona")
        response = client.service.CreatePersona(per_response_dto)

        logger.debug("Respuesta recibida del servicio SOAP")
            
            # Convertir la respuesta en un diccionario JSON y retornar
        return {
                'name': response.name,
                'arcana': response.arcana,
                'level': response.level,
                'stats': {
                    'St': response.stats.St,
                    'Ma': response.stats.Ma,
                    'En': response.stats.En,
                    'Ag': response.stats.Ag,
                    'Lu': response.stats.Lu
                },
                'strength': response.strength,
                'weak': response.weak
            }
        
    except Fault as fault:
        # Capturar errores específicos del servicio SOAP
        logger.error(f"SOAP Fault: {fault}")
        return None

    except Exception as e:
        # Manejador genérico de errores
        logger.error(f"Error al crear la persona en la API SOAP: {e}")
        return None

if __name__ == "__main__":
    try:
        # Verificar si una persona existe por Nombre
        persona_name = "Angel"
        logger.info("==> Verificando si una persona existe por Nombre")
        exists = verify_persona(persona_name)
        if exists:
            logger.info(f"Persona con nombre {persona_name} existe.")
        else:
            logger.warning(f"No se encontró ninguna persona con el nombre {persona_name}.")

        # Obtener los detalles de una persona por nombre
        logger.info(f"==> Obteniendo detalles de la persona con nombre: {persona_name}")
        result = get_persona_by_name(persona_name)
        if result:
            logger.info(f"Detalles de la persona obtenidos: {result}")
        else:
            logger.warning(f"No se pudo obtener información para la persona con nombre {persona_name}.")

        # Obtener todas las personas
        logger.info("==> Obteniendo todas las personas registradas")
        result = get_all_personas()
        if result:
            logger.info(f"Lista de personas obtenida: {result}")
        else:
            logger.warning("No se pudo obtener la lista de personas.")

        # Buscar persona por nombre
        name = "Alice"
        logger.info(f"==> Buscando persona por nombre: {name}")
        result = get_persona_by_name(name)
        if result:
            logger.info(f"Persona encontrada: {result}")
        else:
            logger.warning(f"No se encontró ninguna persona con el nombre {name}.")

        # Eliminar una persona
        logger.info(f"==> Intentando eliminar persona con nombre: {persona_name}")
        result = delete_persona(persona_name)
        if result:
            logger.info("Persona eliminada exitosamente.")
        else:
            logger.warning(f"No se pudo eliminar la persona con nombre {persona_name}.")

        # Crear una nueva persona
        logger.info("==> Creando una nueva persona")
        PerResponseDto = {
            "name": "Test Persona",
            "arcana": "Fool",
            "level": 5,
            "stats": {'St': 10, 'Ma': 8, 'En': 9, 'Ag': 7, 'Lu': 6},
            "strength": ["Fire"],
            "weak": ["Ice"]
        }
        result = create_persona(
            name=PerResponseDto["name"],
            arcana=PerResponseDto["arcana"],
            level=PerResponseDto["level"],
            stats=PerResponseDto["stats"],
            strength=PerResponseDto["strength"],
            weak=PerResponseDto["weak"]
        )
        if result:
            logger.info(f"Persona creada exitosamente: {result}")
        else:
            logger.warning("No se pudo crear la persona.")

    except Exception as e:
        logger.error(f"Ocurrió un error inesperado en la ejecución principal: {e}")

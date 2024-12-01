import logging
from requests import Session
from zeep import Client
from zeep.transports import Transport
from zeep.exceptions import Fault
from zeep.settings import Settings

# Configurar el logger
logging.basicConfig(level=logging.DEBUG)
logger = logging.getLogger(__name__)

def verify_persona(persona_id):
    """
    Verifica si una persona existe a través de un servicio SOAP.

    Args:
        persona_id (str): El ID de la persona a verificar.

    Returns:
        bool: True si la persona existe, False en caso de error.
    """
    # Configurar la sesión y el transporte para usar requests
    session = Session()
    transport = Transport(session=session)
    settings= Settings(strict=False)

    try:
        # Crear cliente SOAP
        logger.debug("Creando cliente SOAP con el WSDL")
        client = Client('http://personas-api-svc:8080/PerService.svc?wsdl', transport=transport, settings=settings)

        # Llamar al servicio SOAP con el ID de la persona
        logger.debug(f"Llamando al servicio SOAP con persona_id={persona_id}")
        response = client.service.GetById(persona_id)

        # Procesar la respuesta (verificación básica)
        logger.debug("Respuesta recibida del servicio SOAP")
        return response is not None  # Verifica si la respuesta no está vacía

    except Fault as fault:
        # Capturar errores específicos del servicio SOAP
        logger.error(f"SOAP Fault: {fault}")
        return False

    except Exception as e:
        # Manejador genérico de errores
        logger.error(f"Error al verificar la persona: {e}")
        return False
    
def create_persona_soap(name, arcana, level, stats, strength, weak):
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
        client = Client('http://host.docker.internal:8080/PerService.svc?wsdl', transport=transport, settings=settings)

        # Llamar al servicio SOAP para crear una persona
        logger.debug("Llamando al servicio SOAP para crear persona")
        response = client.service.CreatePerson(name, arcana, level, stats, strength, weak)

        logger.debug("Respuesta recibida del servicio SOAP")
        return response

    except Fault as fault:
        # Capturar errores específicos del servicio SOAP
        logger.error(f"SOAP Fault: {fault}")
        return None

    except Exception as e:
        # Manejador genérico de errores
        logger.error(f"Error al crear la persona en la API SOAP: {e}")
        return None
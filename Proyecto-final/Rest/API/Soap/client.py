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
        client = Client('http://host.docker.internal:8081/PerService.svc?wsdl', transport=transport, settings=settings)

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
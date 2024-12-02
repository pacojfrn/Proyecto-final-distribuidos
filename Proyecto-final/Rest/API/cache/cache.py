import redis

redis_client = redis.StrictRedis(host = 'redis-service.bagm-databases.svc.cluster.local', port=6379, db=0)

def clear_product_quantity_cache():
    try:
        for key in redis_client.scan_iter("users_quantity_*"):
            redis_client.delete(key)
    except Exception as e:
        print(f"Error clearing user quantity cache: {str(e)}")


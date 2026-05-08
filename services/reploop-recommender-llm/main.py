import logging

from fastapi import FastAPI
from router import router
from dotenv import load_dotenv

# Background thread'lerin (discover LLM enrichment) log'larini stdout'a yaz.
# Bu olmadan logger.warning/info/error mesajlari uvicorn tarafindan yutuluyor.
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s %(levelname)s %(name)s: %(message)s",
)

load_dotenv()

app = FastAPI(
    title="RepLoop Recommendation Engine",
    description="Icerik tabanli filtreleme ile kisisellestirilmis antrenman onerileri",
    version="1.0.0",
)

app.include_router(router)

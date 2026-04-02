from fastapi import FastAPI
from router import router
from dotenv import load_dotenv

load_dotenv()

app = FastAPI(
    title="RepLoop Recommendation Engine",
    description="Icerik tabanli filtreleme ile kisisellestirilmis antrenman onerileri",
    version="1.0.0",
)

app.include_router(router)

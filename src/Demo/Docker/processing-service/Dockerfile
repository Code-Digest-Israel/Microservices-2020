FROM python:3.6-slim-buster
COPY . /app
WORKDIR /app
RUN apt-get update
RUN apt-get -y install libgeos-dev
RUN pip install -r requirements.txt
EXPOSE 5002
ENTRYPOINT ["python"]
CMD ["src/app.py"]


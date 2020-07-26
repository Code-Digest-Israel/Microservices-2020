import flask
from flask import request, jsonify
from flask_cors import CORS
import sys
import os
import requests
import json
json_file = open('./data.json',)
outrage_data = json.loads(json_file.read())

app = flask.Flask(__name__)
CORS(app)

port = 5002
prodcast_url = "http://output-service:80/api/notifications"

print(f"Ruuning flask service on port {port}...", flush=True)


@app.route('/process', methods=['POST'])
def process():
    print("Processinggg patients in city: {city}, added:{added}")
    content = request.json
    (city, added) = (str(content['city']), int(content['added']))
    print(f"Processing patients in city: {city}, added:{added}", flush=True)
    existing_patients = get_existing_patients(city)
    outrage_data[city] = added + existing_patients
    
    notifyingResponse = prodcast_outbreak(prodcast_url,city,outrage_data[city] );

    return jsonify(success=True)


def prodcast_outbreak(prodcast_url, city, updated_patients):
    headers = {
        'Accept': 'application/json',
        'Content-Type': 'application/json',        
    }
    data= {'City': city, 'NumOfPatients': updated_patients}
    response = requests.post("http://output-service:80/api/notifications",json=data)
    print(f"response: {response}")
    return "true"


def get_existing_patients( city):
    print(f"get existing patients in {city} from {outrage_data}", flush=True)
    return int(outrage_data[city])


def delete_patients(state_url, city):
    
    response = requests.delete(url=f"{state_url}/{city}")
    print(f"deleting statusCode = {response.status_code}", flush=True)

    if(response.ok and response.text):
        print(f"result text = {response.text}", flush=True)


app.run(host='0.0.0.0', port='5002')

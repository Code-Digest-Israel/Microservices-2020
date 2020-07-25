import flask
from flask import request, jsonify
from flask_cors import CORS
import sys
import os
import requests
import json
json_file = open('./data.json',) 

app = flask.Flask(__name__)
CORS(app)

dapr_port = os.getenv("DAPR_HTTP_PORT", 4002)
port = 5002

base_url = f"http://localhost:{dapr_port}/v1.0"

state_store_name = 'corona_state'
state_url = f"{base_url}/state/{state_store_name}"

prodcast_method_name = 'outbreak'
prodcast_url = "http://output-service:5003/api/notifications"

print(f"Ruuning flask service on port {port}...", flush=True)


@app.route('/process', methods=['POST'])
def process():
    print("Processing patients in city: {city}, added:{added}")
    content = request.json
    (city, added) = (str(content['city']), int(content['added']))

    print(f"Processing patients in city: {city}, added:{added}", flush=True)

    existing_patients = get_existing_patients(state_url, city)
    updated_patients = added + existing_patients
    print(f"Added oatients: {updated_patients}, added:{added}")

    if(updated_patients > 100):
	    prodcast_outbreak(prodcast_url, city, updated_patients)

    return jsonify(success=True)


def prodcast_outbreak(prodcast_url, city, updated_patients):
    print(f"prodcasting outbreak for cityy '{city}' with '{prodcast_url}' patients", flush=True)


    # headers = {
    #     'Accept': 'application/json',
    #     'Content-Type': 'application/json',        
    # }
    print(f"prodcast url = {prodcast_url}")

    response = requests.post( "http://output-service:5003/api/notifications", { 'city': city, 'patients': updated_patients })
    print(f"prodcast outbreak statusCode = {response.status_code}", flush=True)
    
def update_patients(state_url, city, updated_patients):
    print(f"New patients value is '{updated_patients}'", flush=True)

    data = [{ 
        'key': city,
        'value': {
            'city': city, 
            'patients': updated_patients
        }
    }]

    headers = {
        'Accept': 'application/json',
        'Content-Type': 'application/json',        
    }
    
    response = requests.post(url = state_url, data = json.dumps(data), headers = headers)

    if(response.ok == False):
        print(f"result text = {response.text}", flush=True)

    print(f"save update statusCode = {response.status_code}", flush=True)

def get_existing_patients(state_url, city):
    data = json.loads(json_file.read())
    print(f"get existing patients {data}", flush=True)
    json_file.close()
    return int(data["haifa"])
    
    

def delete_patients(state_url, city):

    response = requests.delete(url = f"{state_url}/{city}")
    print(f"deleting statusCode = {response.status_code}", flush=True)

    if(response.ok and response.text):
        print(f"result text = {response.text}", flush=True)

app.run(host = '0.0.0.0',port='5002')
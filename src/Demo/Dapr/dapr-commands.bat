dapr run --app-id input-service --app-port 5001 --port 4001 --config demo/corona/components/tracing.yaml --components-path demo/corona/components node demo/corona/input-service/src/index.js
dapr run --app-id processing-service --app-port 5002 --port 4002 --config demo/corona/components/tracing.yaml --components-path demo/corona/components py demo/corona/processing-service/src/app.py
dapr run --app-id output-service --app-port 5003 --port 4003 --config demo/corona/components/tracing.yaml --components-path demo/corona/components demo\Corona\output-service\run.bat
































--max-concurrency 1


dapr run --app-id input-service --app-port 5001 --port 4001 node demo/corona/input-service/src/index.js
dapr run --app-id processing-service --app-port 5002 --port 4002 py demo/corona/processing-service/src/app.py
dapr run --app-id output-service --app-port 5003 --port 4003 demo\Corona\output-service\run.bat

dapr run --app-id input-service --app-port 5001 --port 4001 --components-path demo/corona/components node demo/corona/input-service/src/index.js
dapr run --app-id processing-service --app-port 5002 --port 4002 --components-path demo/corona/components py demo/corona/processing-service/src/app.py
dapr run --app-id output-service --app-port 5003 --port 4003 --components-path demo/corona/components demo\Corona\output-service\run.bat


dze3p71vloic3jgs4zsh7iuhwcaoe0rmu3izjunh
docker run -e APPINSIGHTS_INSTRUMENTATIONKEY=5b38b06b-c37d-4442-af76-8942e3e337c5 -e APPINSIGHTS_LIVEMETRICSSTREAMAUTHENTICATIONAPIKEY=pv74nctwdtka3r52pnxxe5mil475at2re2r7eawf -d -p 55678:55678 daprio/dapr-localforwarder:latest

dapr run --app-id input-service --app-port 5001 --port 4001 --config demo/corona/components/tracing.yaml node demo/corona/input-service/src/index.js
dapr run --app-id processing-service --app-port 5002 --port 4002 --config demo/corona/components/tracing.yaml py demo/corona/processing-service/src/app.py
dapr run --app-id output-service --app-port 5003 --port 4003 --config demo/corona/components/tracing.yaml demo\Corona\output-service\run.bat

import { AzureFunction, Context, HttpRequest } from "@azure/functions"
import fetch from "node-fetch"

const httpTrigger: AzureFunction = async function (context: Context, req: HttpRequest): Promise<void> {
  context.log("HTTP trigger function processed a request.")

  const city = req.body.city
  const added = req.body.added

  sendData({ city, added })

  context.res = {
    status: 200,
    body: { city, added },
  }
}

function sendData(data) {
  console.log("Sending data to process service...")
  const processingServiceUrl = process.env["ProcessingServiceUrl"]

  fetch(processingServiceUrl, {
    method: "POST",
    body: JSON.stringify(data),
    headers: {
      "Content-Type": "application/json",
    },
  })
}

export default httpTrigger

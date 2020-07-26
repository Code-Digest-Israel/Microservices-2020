const express = require("express");
const https = require("isomorphic-fetch");
const app = express();

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

const port = 5001;

app.post("/corona", function (req, res) {
  const city = req.body.city;
  const added = req.body.added;

  sendData({ city, added });

  res.send({ city, added });
  console.log(`hello, corona.  City: ${city}, added: ${added}`);
});

app.set("port", port);

app.listen(port, () => {
  console.log(
    `Server is up and running at http://localhost:${port} in ${app.get(
      "env"
    )} mode`
  );
});

function sendData(data) {
  console.log("Sending data to process service...");
  fetch(`http://processing-service:5002/process`, {
    method: "POST",
    body: JSON.stringify(data),
    headers: {
      "Content-Type": "application/json",
    },
  });
}

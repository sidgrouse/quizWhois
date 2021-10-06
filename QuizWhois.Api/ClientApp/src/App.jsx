import './App.css';
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
//import { Button, Input, notification } from "antd";
import React, { useEffect, useState } from "react";


function App() {

  const [connection, setConnection] = useState(null);
  const [inputText, setInputText] = useState("");

  useEffect(() => {
    const connect = new HubConnectionBuilder()
      .withUrl("https://localhost:5001/hub")
      .withAutomaticReconnect()
      .build();

    setConnection(connect);
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          connection.on("ReceiveAnswer", (id, isAnswerRight) => {
            console.log(id,isAnswerRight)
            // notification.open({
            //   message: "New Notification",
            //   description: question,
            // });
          });
          connection.on("ReceiveQuestion", (id, question) => {
            console.log(id,question)
            // notification.open({
            //   message: "New Notification",
            //   description: question,
            // });
          });
        })
        .catch((error) => console.log(error));
    }
  }, [connection]);

  const sendAnswer = async () => {
    if (connection) await connection.send("SendAnswer", 1, "string", inputText);
    setInputText("");
  };

  const sendQuestion = async () => {
    if (connection) await connection.send("SendQuestion");
    setInputText("");
  };


  return (

    <>
    <div className="messages">
   </div>
    <div className="input-zone">
      <input className="input-zone-input"
        value={inputText}
        onChange={(input) => {
          setInputText(input.target.value);
        }}
      />
      <button onClick={sendAnswer} type="primary">
        Send
      </button>
      <button onClick={sendQuestion} type="primary">
        Get Question
      </button>
      </div>
    </>

  // <div id="divMessages" class="messages">
  // </div>
  // <div class="input-zone">
  //     <label id="lblMessage" for="tbMessage">Message:</label>
  //     <input id="tbMessage" class="input-zone-input" type="text" />
  //     <button id="btnSend">Send</button>
  // </div>

    // <div className="App">
    //   <header className="App-header">
    //       Learn React
    //   </header>
    // </div>
  );
}

export default App;

import './App.css';
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import React, { useEffect, useState } from "react";
import QuestionList from './Components/QuestionList.jsx';

var currentId = 0
var currentQuestion = ''
var key = 0
function App() {

  const [questions, addQuestion] = useState([]);
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
            if (isAnswerRight) {
              addQuestion(prevState => {
                const newState = [...prevState]
                newState[key-1].cssClass = 1
                return newState
              })
            } 
            else 
            {
              addQuestion(prevState => {
                const newState = [...prevState]
                newState[key-1].cssClass = 2
                return newState
              })
            }
          });
          connection.on("ReceiveQuestion", (id, question) => {
            currentId = id
            currentQuestion = question
            key++

            var questionObj = new Object();
            questionObj.id = id;
            questionObj.question = question;
            questionObj.key = key
            questionObj.cssClass = 0
            addQuestion(oldArray => [...oldArray, questionObj]);
          });
        })
        .catch((error) => console.log(error));
    }
  }, [connection]);

  const sendAnswer = async () => {
    if (connection) await connection.send("SendAnswer", currentId, currentQuestion, inputText);
      setInputText("");
  };

  const sendQuestion = async () => {
    if (connection) await connection.send("SendQuestion");
    
  };

  return (
  <div className="wrapper">
    <h1>Questions</h1>
    <div className="input-zone">
      <input className="input-zone-input"
        value={inputText}
        onChange={(input) => {
          setInputText(input.target.value);
        }}
      />
      <button onClick={sendAnswer} type="primary">
        Send Answer
      </button>
      <button onClick={sendQuestion} type="primary">
        Get Question
      </button>
    </div>
    <QuestionList qList={questions} />

  </div>
  );
}

export default App;

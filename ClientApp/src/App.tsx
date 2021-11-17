import './App.css';
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import * as React from "react";
import { QuestionList } from './Components/QuestionList';
import { Answer, IQuestion } from './interfaces'


let currentId: number = 0
let key: number = 0

const App: React.FC = () => {

  const [questions, addQuestion] = React.useState<IQuestion[]>([]);
  const [connection, setConnection] = React.useState(null);
  const [inputText, setInputText] = React.useState<string>("");
  
  React.useEffect(() => {
    const connect = new HubConnectionBuilder()
      .withUrl("https://localhost:5001/hub")
      .withAutomaticReconnect()
      .build();
      console.log('connect')
    setConnection(connect);
  }, []);

  React.useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          connection.on("ReceiveAnswer", (id: number, isAnswerRight: string) => {
            if (isAnswerRight) {
              addQuestion(prevState => {
                const newState = [...prevState]
                newState[key-1].cssClass = Answer.right
                return newState
              })
            } 
            else 
            {
              addQuestion(prevState => {
                const newState = [...prevState]
                newState[key-1].cssClass = Answer.wrong
                return newState
              })
            }
          });
          connection.on("ReceiveQuestion", (id: number, questionText: string) => {
            console.log('receiveQ')
            currentId = id
            key++
            console.log(id,questionText)
            let questionObj: IQuestion = {id : id, text: questionText, key : key, cssClass: Answer.default};
            console.log('xth1')
            addQuestion(oldArray => [...oldArray, questionObj]);
          });
        })
        .catch((error: any) => console.log(error));
    }
  }, [connection]);

  const sendAnswer = async () => {
    if (connection) await connection.send("SendAnswer", currentId, inputText);
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
      <button onClick={sendAnswer} className="primary">
        Send Answer
      </button>
      <button onClick={sendQuestion} className="primary">
        Get Question
      </button>
    </div>
    <QuestionList qList={questions} />

  </div>
  );
}

export default App;

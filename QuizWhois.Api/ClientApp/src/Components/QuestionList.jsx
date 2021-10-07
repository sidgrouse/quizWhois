import React from "react";
import QuestionItem from './QuestionItem.jsx';

export default function QuestionList(props) {
    return (
        <ul>
            {
                props.qList.map(question => {return <QuestionItem q={question} key={question.key}/>})
            }
        </ul>
    )
}

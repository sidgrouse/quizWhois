import * as React from "react";
import { QuestionItem }  from './QuestionItem';
import { IQuestion } from '../interfaces'

interface QuestionListProps {
    qList: IQuestion[]
}
export const QuestionList: React.FC<QuestionListProps> = (props) => {
    return (
        <ul>
            {
                props.qList.map(question => {return <QuestionItem qItem={question} key={question.key}/>})
            }
        </ul>
    )
}

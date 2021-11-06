import * as React from "react";
import { IQuestion, Answer } from '../interfaces'

interface QuestionItemProps {
    qItem: IQuestion,
    key: number
}
export const QuestionItem: React.FC<QuestionItemProps> = (props) => {
    const classes = ['list-group-item']
    if(props.qItem.cssClass == Answer.right)
    {
        classes.push('rightAnswer')
    }
    if(props.qItem.cssClass == Answer.wrong)
    {
        classes.push('wrongAnswer')
    }
    return <li className={classes.join(' ')}>{props.qItem.text}</li>
}

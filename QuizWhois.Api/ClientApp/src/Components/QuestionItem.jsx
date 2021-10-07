import React from "react";

export default function QuestionItem(props) {
    const classes = ['list-group-item']
    if(props.q.cssClass == 1)
    {
        classes.push('rightAnswer')
    }
    if(props.q.cssClass == 2)
    {
        classes.push('wrongAnswer')
    }
    return <li className={classes.join(' ')}>{props.q.question}</li>
}
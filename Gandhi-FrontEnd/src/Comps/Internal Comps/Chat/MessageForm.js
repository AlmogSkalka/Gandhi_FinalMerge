import React from 'react'
import * as FiIcons from "react-icons/fi";

export default function MessageForm({ handleSubmit, text, setText, setImg }) {
    return (
        <form className="message_form" onSubmit={handleSubmit}>
            <div>
                <input
                    type="text"
                    placeholder='קשקשו משהו פה...'
                    value={text}
                    onChange={e => setText(e.target.value)} />
            </div>
            <div>
                <button style={{ marginTop: '10px' }} type='submit' disabled={text.length >= 1 ? false : true}>
                    <FiIcons.FiSend />
                </button>
            </div>
        </form>
    )
}

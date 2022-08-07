import React from 'react'
import { useNavigate } from 'react-router-dom';
import * as AiIcons from "react-icons/ai";
import { IconContext } from 'react-icons';


export default function AddNewItemBtnComp() {
    const navigate = useNavigate()

    const AddItemPage = () => {
        navigate("/NewItemPage")
    }

    return (
        <>
            <IconContext.Provider value={{
                color: '#4A90E2', size: '3em', position: 'fixed',
                display: 'absolute'
            }}>
                <AiIcons.AiFillPlusCircle
                    onClick={AddItemPage}
                    id='addItemBtn' />
            </IconContext.Provider>
        </>


    )
}
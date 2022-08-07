import React from 'react'
import ItemComp from '../Comps/Internal Comps/Item/ItemComp';
import { useLocation } from 'react-router-dom';

export default function ItemPage() {
    const { state } = useLocation();

    return (
        <div className='pages'>
            <ItemComp Item={state} />
        </div>
    )
}

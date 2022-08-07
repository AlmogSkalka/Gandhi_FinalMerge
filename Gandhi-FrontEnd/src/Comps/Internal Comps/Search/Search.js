import React, { useState } from 'react';
import Scroll from './Scroll';
import SearchList from './SearchList';

export default function Search({ details }) {

    const [searchField, setSearchField] = useState("");

    const filteredItems = details.filter(
        Item => {
            return (
                Item
                    .ItemDesc
                    .includes(searchField.toLowerCase()) ||

                Item
                    .Category
                    .includes(searchField.toLowerCase()) ||

                Item
                    .Color
                    .includes(searchField.toLowerCase()) ||

                Item
                    .Brand
                    .includes(searchField)
            )
        }
    )


    const handleChange = e => {
        setSearchField(e.target.value);
    };

    function searchList() {
        return (
            <Scroll>
                <SearchList filteredItems={filteredItems} />
            </Scroll>
        );
    }

    return (

        <>


            <section className="garamond">
                <div className="navy georgia ma0 grow">
                    <h2 className="f2">חיפוש פריט</h2>
                </div>
                <div className="pa2">
                    <input
                        className="pa3 bb br3 grow b--none bg-lightest-blue ma3"
                        type="search"
                        placeholder="?שם פריט"
                        onChange={handleChange}
                        style={{ textAlign: 'center' }}
                    />
                    <br /><br />
                </div>
                {searchList()}
            </section>
        </>
    );
}
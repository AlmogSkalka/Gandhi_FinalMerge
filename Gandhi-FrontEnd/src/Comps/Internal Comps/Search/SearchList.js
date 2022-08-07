import React from "react";
import SearchCard from "./SearchCard";
import "./SearchList.css";

export default function SearchList({ filteredItems }) {
  const filtered = filteredItems.map((Item) => (
    <SearchCard key={Item.ItemId} Item={Item} />
  ));

  return <div className="search-results">{filtered}</div>;
}
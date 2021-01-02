import React from 'react';
import './NotesTable.scss';
import { MealType } from '../../models';
import { NotesTableStateToPropsMapResult } from './NotesTableConnected';
import { Table, TableColumn } from '../__ui__';
import NotesTableRowConnected from '../NotesTableRow';

interface NotesTableProps extends NotesTableStateToPropsMapResult {
  mealType: MealType;
}

const notesTableColumns = [
  <TableColumn key="Product" name="Product" width="50%"></TableColumn>,
  <TableColumn key="Quantity" name="Quantity" width="20%"></TableColumn>,
  <TableColumn key="Calories" name="Calories"></TableColumn>,
  <TableColumn key="Edit" name="" width="35px"></TableColumn>,
  <TableColumn key="Delete" name="" width="35px"></TableColumn>,
];

const NotesTable: React.FC<NotesTableProps> = ({ mealType, noteItems, notesForMealFetchStates }: NotesTableProps) => {
  const mapNoteItemsToTableRows = (): JSX.Element[] => {
    const rows: JSX.Element[] = [];
    const notesForMeal = noteItems.filter(n => n.mealType === mealType);

    notesForMeal.forEach(note => {
      rows.push(<NotesTableRowConnected note={note}></NotesTableRowConnected>);
    });

    return rows;
  };

  const targetNotesFetchState = notesForMealFetchStates.find(s => s.mealType === mealType);
  const targetNotesErrorMessage = targetNotesFetchState && targetNotesFetchState.error;

  return (
    <div className="notes-table">
      <Table
        columns={notesTableColumns}
        rows={mapNoteItemsToTableRows()}
        dataErrorMessage={targetNotesErrorMessage}
        isCondensed={true}
      ></Table>
    </div>
  );
};

export default NotesTable;
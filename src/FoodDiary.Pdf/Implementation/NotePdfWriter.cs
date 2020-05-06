﻿using System;
using FoodDiary.Domain.Entities;
using FoodDiary.Domain.Services;
using FoodDiary.Pdf.Services;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace FoodDiary.Pdf.Implementation
{
    internal class NotePdfWriter : INotePdfWriter
    {
        private readonly ICaloriesService _caloriesService;

        public NotePdfWriter(ICaloriesService caloriesService)
        {
            _caloriesService = caloriesService ?? throw new ArgumentNullException(nameof(caloriesService));
        }

        public void WriteNoteToNotesTable(Table notesTable, Note note)
        {
            var caloriesCount = Convert.ToInt32(Math.Floor(
                _caloriesService.CalculateForQuantity(note.Product.CaloriesCost, note.ProductQuantity)));

            var row = notesTable.AddRow();
            row.Height = $"{PagesPdfGeneratorOptions.NotesTableRowHeightCentimeters}cm";
            row.Format.Alignment = ParagraphAlignment.Center;
            row.VerticalAlignment = VerticalAlignment.Center;

            row.Cells[PagesPdfGeneratorOptions.ProductNameColumnIndex]
                .AddParagraph(note.Product.Name);

            row.Cells[PagesPdfGeneratorOptions.ProductQuantityColumnIndex]
                .AddParagraph(note.ProductQuantity.ToString());

            row.Cells[PagesPdfGeneratorOptions.CaloriesCountColumnIndex]
                .AddParagraph(caloriesCount.ToString());
        }
    }
}
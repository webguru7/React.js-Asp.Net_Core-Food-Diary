﻿using FoodDiary.Domain.Entities;
using FoodDiary.Domain.Enums;
using MediatR;

namespace FoodDiary.Application.Notes.Requests
{
    public class MoveNoteRequest : IRequest<int>
    {
        public Note NoteForMove { get; set; }

        public MealType DestMeal { get; set; }

        public int Position { get; set; }
    }
}

﻿using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FoodDiary.Domain.Dtos;

namespace FoodDiary.Import.UnitTests.Customizations
{
    class JsonNotesWithValidInfoCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var product1 = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, "Valid product name 1")
                    .With(p => p.CaloriesCost, 150)
                    .With(p => p.Category, "Valid category name 1")
                    .Create();
                var product2 = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, "Valid product name 1")
                    .With(p => p.CaloriesCost, 200)
                    .With(p => p.Category, "Valid category name 2")
                    .Create();
                var product3 = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, "Valid product name 2")
                    .With(p => p.CaloriesCost, 120)
                    .With(p => p.Category, "Valid category name 2")
                    .Create();

                var note1 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, product1)
                    .Create();
                var note2 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, product2)
                    .Create();
                var note3 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, product3)
                    .Create();

                return new List<NoteJsonItemDto>() { note1, note2, note3 }
                    .AsEnumerable();
            });
        }
    }

    class JsonNotesWithInvalidProductCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var productWithLongName = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, new string(fixture.Create<char>(), 65))
                    .With(p => p.CaloriesCost, 100)
                    .With(p => p.Category, "Valid category name")
                    .Create();
                var productWithShortName = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, new string(fixture.Create<char>(), 1))
                    .With(p => p.CaloriesCost, 100)
                    .With(p => p.Category, "Valid category name")
                    .Create();
                var productWithZeroCaloriesCost = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, "Valid product name")
                    .With(p => p.CaloriesCost, 0)
                    .With(p => p.Category, "Valid category name")
                    .Create();
                var productWithHugeCaloriesCost = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, "Valid product name")
                    .With(p => p.CaloriesCost, 1001)
                    .With(p => p.Category, "Valid category name")
                    .Create();
                var productWithNullName = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, null as string)
                    .With(p => p.CaloriesCost, 100)
                    .With(p => p.Category, "Valid category name")
                    .Create();

                var note1 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, productWithLongName)
                    .Create();
                var note2 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, productWithShortName)
                    .Create();
                var note3 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, productWithZeroCaloriesCost)
                    .Create();
                var note4 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, productWithHugeCaloriesCost)
                    .Create();
                var note5 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, productWithNullName)
                    .Create();

                return new List<NoteJsonItemDto>() { note1, note2, note3, note4, note5 }
                    .AsEnumerable();
            });
        }
    }

    class JsonNotesWithInvalidCategoryCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var productWithShortCategoryName = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, "Valid product name")
                    .With(p => p.CaloriesCost, 150)
                    .With(p => p.Category, new string(fixture.Create<char>(), 1))
                    .Create();
                var productWithLongCategoryName = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, "Valid product name")
                    .With(p => p.CaloriesCost, 200)
                    .With(p => p.Category, new string(fixture.Create<char>(), 65))
                    .Create();
                var productWithNullCategoryName = fixture.Build<ProductJsonItemDto>()
                    .With(p => p.Name, "Valid product name")
                    .With(p => p.CaloriesCost, 200)
                    .With(p => p.Category, null as string)
                    .Create();

                var note1 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, productWithShortCategoryName)
                    .Create();
                var note2 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, productWithLongCategoryName)
                    .Create();
                var note3 = fixture.Build<NoteJsonItemDto>()
                    .With(n => n.Product, productWithNullCategoryName)
                    .Create();

                return new List<NoteJsonItemDto>() { note1, note2, note3 }
                    .AsEnumerable();
            });
        }
    }
}
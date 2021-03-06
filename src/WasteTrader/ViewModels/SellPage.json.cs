using Starcounter;
using WasteTrader.Database;
using Simplified.Ring3;
using WasteTrader.Helpers;
using System;

namespace WasteTrader.ViewModels
{
    partial class SellPage : Json
    {
        public bool ValidInput => 
            this.Waste.Description.IsValid &&
            this.Waste.Title.IsValid &&
            this.Waste.Price.IsValid;

        void Handle(Input.SubmitTrigger action)
        {
            if (ValidInput)
            {
                var waste = this.CreateWaste();
                this.WasteKey = waste.Key;
                this.ClearViewModel();
                this.SubmitMessage = "";
            }
            else
            {
                this.SubmitMessage = "Du har inte fyllt i alla f�lt riktigt";
            }
        }

        private Waste CreateWaste()
        {
            return Db.Transact(() =>
            {
                return new Waste()
                {
                    Title = this.Waste.Title.Value,
                    Description = this.Waste.Description.Value,
                    Price = this.Waste.Price.Value,
                    User = SystemUser.GetCurrentSystemUser(),
                    Unit = (Unit)this.Waste.Unit,
                    Quantity = this.Waste.Quantity,
                    Active = true
                };
            });
        }

        private void ClearViewModel()
        {
            this.Waste.Description.Value = "";
            this.Waste.Title.Value = "";
            this.Waste.Price.Value = 0;
            this.Waste.Quantity = 0;
        }

        [SellPage_json.Waste.Title]
        partial class WasteTitle : Json
        {
            private const int MinLength = 3;
            private const int MaxLength = 100;
            private string InvalidTitleWarning = $"Titeln m�ste vara mellan {MinLength} och {MaxLength} tecken l�ngt";

            void Handle(Input.Value action)
            {
                this.IsValid = new StringValidation(action.Value)
                    .ValidateLength(MinLength, MaxLength)
                    .IsValid;

                this.Message = this.IsValid ? string.Empty : InvalidTitleWarning;
            }
        }

        [SellPage_json.Waste.Description]
        partial class WasteDescription : Json
        {
            private const int MaxLength = 2000;
            private string InvalidDescriptionWarning = $"Beskrivningen kan inte vara mer �n {MaxLength} tecken l�ngt";

            void Handle(Input.Value action)
            {
                this.IsValid = new StringValidation(action.Value)
                    .ValidateLength(0, MaxLength)
                    .IsValid;

                this.Message = this.IsValid ? string.Empty : InvalidDescriptionWarning;
            }
        }

        [SellPage_json.Waste.Price]
        partial class WastePrice : Json
        {
            private string InvalidPriceWarning = "Priset m�ste vara h�gre �n noll";

            void Handle(Input.Value action)
            {
                this.IsValid = new NumberValidation(action.Value)
                    .IsMoreThanZero()
                    .IsValid;

                this.Message = this.IsValid ? string.Empty : InvalidPriceWarning;
            }
        }
    }
}

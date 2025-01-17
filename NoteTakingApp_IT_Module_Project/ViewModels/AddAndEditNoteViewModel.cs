﻿using Caliburn.Micro;

using NoteTakingApp_IT_Module_Project.Data;
using NoteTakingApp_IT_Module_Project.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NoteTakingApp_IT_Module_Project.ViewModels
{
    public class AddAndEditNoteViewModel : Screen
    {
        private static NoteModel editingDataContext;

        #region Property change handler
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        /// <summary>
        /// The data context for the note that was opened
        /// </summary>
        public static NoteModel EditingDataContext 
        {
            get { return editingDataContext; }
            set 
            { 
                editingDataContext = value;
            }
        }

        /// <summary>
        /// A list of tags suited for checkBox use
        /// </summary>
        public List<CheckBoxTag> CheckBoxTags
        {
            get
            {
                return ShellViewModel.Tags.Select(parent => new CheckBoxTag(parent.Name, parent.ID)).ToList();
            }
           
        }
    }

    /// <summary>
    /// A class meant to enable checking wether a tag from the overall collection belongs to that of the current note
    /// </summary>
    public class CheckBoxTag : TagModel
    {
        #region Property change handler
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Constructors
        public CheckBoxTag()
        {
        }

        public CheckBoxTag(string name, int id)
        {
            Name = name;
            ID = id;
        }
        #endregion

        public bool IsChecked
        {
            get
            {
                //checks if the note contains the current tag
                return AddAndEditNoteViewModel.EditingDataContext.NoteTags.Any(t => t.Name == this.Name);
            }
            set
            {

                if (value)
                {
                    //if the checkBox gets checked, then add the tag from the full list to list of tags for the note
                    AddAndEditNoteViewModel.EditingDataContext.NoteTags.Add(ShellViewModel.Tags.First(t => t.Name == this.Name));
                    NoteData noteData = new NoteData();
                    noteData.AddTagToNote(ShellViewModel.Tags.First(t => t.Name == this.Name), AddAndEditNoteViewModel.EditingDataContext);
                } else
                {
                    //if the chackBox is not checked, then remove said tag from the list of tags for the note
                    var tag = AddAndEditNoteViewModel.EditingDataContext.NoteTags.First(t => t.Name == this.Name);
                    AddAndEditNoteViewModel.EditingDataContext.NoteTags.Remove(tag);
                    NoteData noteData = new NoteData();
                    noteData.RemoveTagToNote(tag, AddAndEditNoteViewModel.EditingDataContext);
                }
                NotifyPropertyChanged();
            }
        }
    }
}

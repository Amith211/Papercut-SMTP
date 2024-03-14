﻿// Papercut
// 
// Copyright © 2008 - 2012 Ken Robertson
// Copyright © 2013 - 2017 Jaben Cargman
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
// http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Papercut.Helpers
{
    using Papercut.Core.Domain.Message;
    using Papercut.Message;

    public class MimeMessageEntry : MessageEntry
    {
        string _subject;

        public MimeMessageEntry(MessageEntry entry, MimeMessageLoader loader)
            : base(entry.File)
        {
            IsSelected = entry.IsSelected;

            //loader.Get(this).Subscribe(m => { Subject = m.Subject; },
            //    e =>
            //    {
            //        Subject = "Failure loading message: " + e.Message;
            //    });
        }

        public string Subject
        {
            get { return _subject; }
            protected set
            {
                _subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }
    }
}
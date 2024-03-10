﻿// Papercut
// 
// Copyright © 2008 - 2012 Ken Robertson
// Copyright © 2013 - 2024 Jaben Cargman
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


namespace Papercut.Rules.Implementations;

[Serializable]
public class ConditionalForwardRule : ForwardRule
{
    string _regexBodyMatch;

    string _regexHeaderMatch;

    [DisplayName("Regex Header Match")]
    public string RegexHeaderMatch
    {
        get { return this._regexHeaderMatch; }
        set
        {
            if (value == this._regexHeaderMatch)
                return;
            this._regexHeaderMatch = value.IsSet() && value.IsValidRegex() ? value : null; ;
            this.OnPropertyChanged(nameof(this.RegexHeaderMatch));
        }
    }

    [DisplayName("Regex Body Match")]
    public string RegexBodyMatch
    {
        get { return this._regexBodyMatch; }
        set
        {
            if (value == this._regexBodyMatch)
                return;

            this._regexBodyMatch = value.IsSet() && value.IsValidRegex() ? value : null;
            this.OnPropertyChanged(nameof(this.RegexBodyMatch));
        }
    }

    [Category("Information")]
    public override string Type => "Conditional Forward";

    protected override IEnumerable<KeyValuePair<string, Lazy<object>>> GetPropertiesForDescription()
    {
        return base.GetPropertiesForDescription().Concat(this.GetProperties());
    }
}
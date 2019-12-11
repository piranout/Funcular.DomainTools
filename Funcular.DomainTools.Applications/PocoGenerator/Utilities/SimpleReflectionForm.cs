#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#endregion

namespace Funcular.DomainTools.Applications.Utilities
{
    /// <summary>
    ///     Form which shows the properties of a custom object as label and edit
    ///     Limited to .net valuetypes
    /// </summary>
    public class SimpleReflectionForm<T> : Form where T : new()
    {
        private readonly Type _objType;
        private readonly T _obj;
        private readonly string[] _propertiesToExclude;
        private readonly bool _enableAddButton;

        private int _currentTabIndex;
        private int _currentTop = 19;
        private int _currentMaxLabelWidth = 220;
        private int _initialHeight;

        public SimpleReflectionForm(T obj, bool enableAddButton)
            : this(obj, enableAddButton, null)
        {
        }

        public SimpleReflectionForm(T obj, bool enableAddButton, params string[] propertiesToExclude)
        {
            InitializeComponent();

            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _propertiesToExclude = propertiesToExclude;
            _enableAddButton = enableAddButton;
            _initialHeight = this.Height;
            _obj = obj;
            _objType = _obj.GetType();
            _dictionary = _obj as Dictionary<string, string>;


            // set button tabs
            btnOk.TabIndex = 998;
            btnCancel.TabIndex = 999;

            if (_enableAddButton && Controls.OfType<MenuItem>().All(x => x.Text != @"Add new"))
            {
                this.Menu = this.Menu ?? new MainMenu();
                this.Menu.MenuItems.Add("Add new", (sender, args) => addElement());
            }

            buildDynamicControls();
        }

        public T Obj
        {
            get { return _obj; }
            //set { this._obj = value; }
        }

        private void buildDynamicControls()
        {
            SuspendLayout();
            Height = this._initialHeight;
            _currentTop = 19;

            if (_obj is ValueType || _obj is string)
            {
                // just show one entry
                Text = string.Format(
                    @"View {0}", _objType
                                    .ToString());
                createLabel(
                    "1", _objType
                            .ToString(), location: new Point(12, _currentTop));
                createValue(null, _obj, _currentTop);
                _currentTop += 23;
                btnOk.Enabled = false;
            }
            //// render dictionary keys as labels & values the same as property values:
            else if (_dictionary != null)
            {
                this.Text = @"Edit known abbreviations dictionary";
                //_dictionary.Clear();
                foreach (var kvp in _dictionary.Where(kvp => !isExcluded(kvp.Key)))
                {
                    createLabel(kvp.Key, kvp.Key, new Point(12, _currentTop));
                    createValue(null, kvp, _currentTop);
                    _currentTop += 23;
                }
            }
            else
            {
                // else show all public properties
                Text = string.Format("Edit {0}", _objType.Name);
                var descriptors = TypeDescriptor.GetProperties(_objType);
                int i = 0;
                var propertyDescriptors = descriptors.OfType<PropertyDescriptor>()
                    .Where(descriptor => descriptor.ComponentType == _objType)
                    .ToArray();
                foreach (PropertyDescriptor descriptor in propertyDescriptors)
                {
                    try
                    {
                        var memberName = descriptor.DisplayName;
                        var memberValue = descriptor.GetValue(_obj);
                        createLabel(memberName, memberName, 12, _currentTop);
                        createValue(memberName, memberValue, _currentTop);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Exception trying to format member index {0} (of {1} total):", i, propertyDescriptors.Length);
                        Debug.WriteLine(e);
                    }
                    i++;
                    _currentTop += 23;
                }
                /*
                                MemberInfo[] members = @type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                                foreach (MemberInfo m in members)
                                {
                                    if (isExcluded(m.Name))
                                        continue;

                                    var f = m as FieldInfo;
                                    var p = m as PropertyInfo;
                                    if (f == null && p == null)
                                        continue;
                                    Type t = f != null ? f.FieldType : p.PropertyType;
                                    if (!t.IsValueType && t != typeof(string))
                                        continue;
                                    createLabel(m, _currentTop);
                                    createValue(m, f != null ? f.GetValue(_obj) : p.GetValue(_obj, null), _currentTop); 
                                    _currentTop += 23;
                                }*/
            }


            // set form size
            Height = _currentTop + 100;
            ResumeLayout();
            btnOk.Location = new Point(this.Width - 150, _currentTop);
            btnCancel.Location = new Point(this.Width - 250, _currentTop);

        }

        private void addElement()
        {
            var dict = _dictionary;
            if (dict == null)
                return;
            var input = Microsoft.VisualBasic.Interaction.InputBox("Enter a key for your new value", "New KeyValuePair");
            if (!string.IsNullOrEmpty(input))
            {
                dict.Add(input, input);
                createLabel(input, input, new Point(12, _currentTop));
                createValue(null, dict.FirstOrDefault(x => x.Key == input), _currentTop);
                _currentTop += 23;
                this.Height += 23;
            }
        }

        private bool isExcluded(string propertyName)
        {
            return _propertiesToExclude != null && _propertiesToExclude.Contains(propertyName);
        }

        private void createValue<TValue>(string memberName, TValue memberValue, int top)
        {
            var p = new Point(_currentMaxLabelWidth, top - 3);
            var s = new Size(_currentMaxLabelWidth + 15, 20);

            Control ctl;
            if (memberValue == null)
                ctl = createTextBox(memberName, "", p, s);
            else if (memberValue is DateTime)
                ctl = createDateTimePicker(memberName, (DateTime)(object)memberValue, p, s);
            else if (memberValue is bool)
                ctl = createCheckBox(memberName, (bool)(object)memberValue, p, s);
            else if (memberValue is KeyValuePair<string, string>)
            {
                ctl = createTextBox(memberName, ((KeyValuePair<string, string>)(object)memberValue).Value, p, s);
                this.Width = Math.Max(Width, _currentMaxLabelWidth + s.Width + 20 + 12);
                var btn = createDeleteButton(memberName, "x", new Point(p.X + ctl.Width + 3, p.Y), new Size(20, 20));
            }
            else if (memberValue is ValueType || memberValue is string)
                ctl = createTextBox(memberName, memberValue.ToString(), p, s);
            else if (memberValue is IEnumerable)
            {
                ctl = createTextBox(memberName, "...", p, s);
                ctl.Enabled = false;
            }
            else
            {
                ctl = createTextBox(memberName, "[unformattable member]", p, s);
                ctl.Enabled = false;
            }
            ctl.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
        }

        private void createValue(MemberInfo m, object value, int top)
        {
            var p = new Point(_currentMaxLabelWidth, top - 3);
            var s = new Size(_currentMaxLabelWidth + 15, 20);

            string name = m == null ?
                value is KeyValuePair<string, string> ?
                    ((KeyValuePair<string, string>)value).Key
                    : _currentTabIndex.ToString(CultureInfo.InvariantCulture)
                : m.Name;
            Control ctl;
            if (value == null)
                ctl = createTextBox(name, "", p, s);
            else if (value is DateTime)
                ctl = createDateTimePicker(name, (DateTime)value, p, s);
            else if (value is bool)
                ctl = createCheckBox(name, (bool)value, p, s);
            else if (value is KeyValuePair<string, string>)
            {
                ctl = createTextBox(name, ((KeyValuePair<string, string>)value).Value, p, s);
                this.Width = Math.Max(Width, _currentMaxLabelWidth + s.Width + 20 + 12);
                var btn = createDeleteButton(name, "x", new Point(p.X + ctl.Width + 3, p.Y), new Size(20, 20));
            }
            else if (value is ValueType || value is string)
                ctl = createTextBox(name, value.ToString(), p, s);
            else if (value is IEnumerable)
            {
                ctl = createTextBox(name, "...", p, s);
                ctl.Enabled = false;
            }
            else
            {
                ctl = createTextBox(name, "unknown", p, s);
                ctl.Enabled = false;
            }
            ctl.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left;
        }

        private Control createDeleteButton(string name, string text, Point location, Size size)
        {
            var button = new Button()
            {
                Location = location,
                Name = "txt" + name,
                Size = size,
                Tag = name,
                Text = text,
                TabIndex = _currentTabIndex++,
                Anchor = AnchorStyles.Top | AnchorStyles.Right

            };
            Controls.Add(button);
            button.Click += deleteButtons_Click;
            return button;
        }

        void deleteButtons_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button == null || !_dictionary.ContainsKey(button.Tag.ToString()))
            {
                return;
            }
            removeDynamicControls();
            _dictionary.Remove(button.Tag.ToString());
            buildDynamicControls();
        }

        private void removeDynamicControls()
        {
            List<Control> c = Controls
                .OfType<TextBox>()
                .Union(Controls.OfType<Label>().Cast<Control>().Where(ctl => _dictionary.ContainsKey(ctl.Tag.ToString()))
                .Union(Controls.OfType<Button>().Where(b => !b.Text.StartsWith("&")
                    && !b.Text.EndsWith("Ok")
                    && !b.Text.EndsWith("Cancel")))
                )
                .ToList();
            this.SuspendLayout();
            foreach (var control in c)
            {
                this.Controls.Remove(control);
            }
            this.ResumeLayout();
        }

        private Control createTextBox(string name, string text, Point location, Size size)
        {
            var t = new TextBox
                {
                    Location = location,
                    Name = "txt" + name,
                    Size = new Size(Width - (location.X + 60), size.Height),
                    Tag = name,
                    Text = text,
                    TabIndex = _currentTabIndex++
                };
            Controls.Add(t);
            return t;
        }

        private Control createDateTimePicker(string name, DateTime @value, Point location, Size size)
        {
            var d = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Location = location,
                    Tag = name,
                    Name = "dtp" + name,
                    Size = new Size(Width - (location.X + 20), size.Height)
                };
            d.Value = @value < d.MinDate ? d.MinDate : @value;
            d.TabIndex = _currentTabIndex++;
            Controls.Add(d);
            return d;
        }

        private Control createCheckBox(string name, bool @value, Point location, Size size)
        {
            var c = new CheckBox
                {
                    Location = new Point(location.X + 10, location.Y),
                    Name = "chk" + name,
                    CheckAlign = ContentAlignment.MiddleLeft,
                    Size = new Size(Width - (location.X + 20), size.Height),
                    TabIndex = _currentTabIndex++,
                    Text = "",
                    Tag = name,
                    UseVisualStyleBackColor = true,
                    Checked = @value
                };
            Controls.Add(c);
            return c;
        }

        private Label createLabel(MemberInfo m, int top)
        {
            Label l = createLabel(m.Name, m.Name, new Point(12, top));
            return l;
        }

        private Label createLabel(string labelName, string labelText, int x, int y)
        {
            var l = new Label
            {
                AutoSize = true,
                Name = "lbl" + labelName,
                Text = labelText,
                Location = new Point(x, y),
                Tag = labelName
            };
            _currentMaxLabelWidth = Math.Max(_currentMaxLabelWidth, l.Width);
            Controls.Add(l);
            return l;
        }
        private Label createLabel(string name, string text, Point location)
        {
            var l = new Label { AutoSize = true, Name = "lbl" + name, Text = text, Location = location, Tag = name};
            _currentMaxLabelWidth = Math.Max(_currentMaxLabelWidth, l.Width);
            Controls.Add(l);
            return l;
        }

        /// <summary>
        /// Validate that all controls have valid values for their corresponding property's type.
        /// </summary>
        /// <returns></returns>
        private bool checkContents()
        {
            if (typeof(T) == typeof(Dictionary<string, string>))
            {
                return true;
            }
            foreach (Control c in Controls)
            {
                if ((c.Tag == null) || (c.Tag.ToString() == string.Empty)) continue;
                string prop = c.Tag.ToString();

                // get member 
                MemberInfo[] ms = typeof(T).GetMember(prop);
                if (ms.Length != 1) continue;
                MemberInfo m = ms[0];
                if (m == null) continue;
                var f = m as FieldInfo;
                var p = m as PropertyInfo;
                if (f == null && p == null) continue;
                Type t = f != null ? f.FieldType : p.PropertyType;
                if (!t.IsValueType && t != typeof(string)) continue;
                if ((t == typeof(DateTime)) || (t == typeof(bool))) continue;
                if (validateControlValue(t, c.Text))
                    continue;
                c.Focus();
                c.Select();
                MessageBox.Show(text: string.Format(format: "{0} has an invalid value.", arg0: prop), caption: @"Error in value", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //save all to the mObj
            if (_obj is ValueType || _obj is string) return;
            if (!checkContents())
            {
                DialogResult = DialogResult.None;
                return;
            }
            foreach (Control c in Controls.OfType<TextBox>().Cast<Control>()
                .Union(Controls.OfType<CheckBox>())
                .Union(Controls.OfType<DateTimePicker>()))
            {
                if ((c.Tag == null) || (c.Tag.ToString() == string.Empty)) 
                    continue;
                string prop = c.Tag.ToString();
                if (_dictionary != null)
                {
                    _dictionary[prop] = c.Text;
                    continue;
                }
                // get member 
                MemberInfo[] ms = _objType
                                    .GetMember(prop);
                if (ms.Length != 1) continue;
                MemberInfo m = ms[0];
                if (m == null) continue;
                var f = m as FieldInfo;
                var p = m as PropertyInfo;
                if (f == null && p == null) continue;
                Type t = f != null ? f.FieldType : p.PropertyType;
                if (!t.IsValueType && t != typeof(string)) continue;
                if (t == typeof(DateTime))
                {
                    if (f != null)
                        f.SetValue(_obj, ((DateTimePicker)c).Value);
                    else
                        p.SetValue(_obj, ((DateTimePicker)c).Value, null);
                }
                else
                {
                    if (t == typeof(bool))
                    {
                        if (f != null)
                            f.SetValue(_obj, ((CheckBox)c).Checked);
                        else
                            p.SetValue(_obj, ((CheckBox)c).Checked, null);
                    }
                    else
                    {
                        if (f != null)
                            f.SetValue(_obj, convertToValueType(t, c.Text));
                        else
                            p.SetValue(_obj, convertToValueType(t, c.Text), null);
                    }
                }
            }
        }


        /// <summary>
        /// Ensures that <paramref name="value"/> can be cast to <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool validateControlValue(Type type, string @value)
        {
            if (type == typeof(Guid))
            {
                Guid g;
                return guidTryParse(@value, out g);
            }

            var paramTypes = new Type[2];
            paramTypes[0] = typeof(string);
            paramTypes[1] = type.MakeByRefType();


            MethodInfo mi = type.GetMethod("TryParse", paramTypes);
            if (mi == null)
                return true;

            object[] args = { @value, null };

            // invoke the TryParse method
            return (bool)mi.Invoke(null, args);
        }

        /// <summary>
        /// Try to cast <paramref name="value"/> to <paramref name="type"/>,
        /// using Convert.To(x) for primitive value types like Int32, etc.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object convertToValueType(Type type, string @value)
        {
            if (type == typeof(Guid))
                return new Guid(@value);

            var paramTypes = new Type[1];
            paramTypes[0] = typeof(string);

            MethodInfo mi = type.GetMethod("Parse", paramTypes);
            if (mi == null)
                return @value;

            object[] args = { @value };

            // invoke the Parse method
            return mi.Invoke(null, args);
        }

        /// <summary>
        /// Returns a Guid if the string can be parsed into one. Otherwise returns an empty Guid. 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool guidTryParse(string s, out Guid result)
        {
            // code from Colin Bowern (http://rockstarguys.com/blogs/colin/archive/2006/01/18/guid-tryparse.aspx)
            if (s == null)
                throw new ArgumentNullException("s");
            var regex = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
            Match match = regex.Match(s);
            if (match.Success)
            {
                result = new Guid(s);
                return true;
            }
            else
            {
                result = Guid.Empty;
                return false;
            }
        }

        #region "generated code"

        private Button btnCancel;
        private Button btnOk;
        private readonly IContainer components = null;
        private Dictionary<string, string> _dictionary;

        /// <summary>
        ///     Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        ///     This method is required for Windows Forms designer support.
        ///     Do not change the method contents inside the source code editor. The Forms designer might
        ///     not be able to load this method if it was changed manually.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk = new Button();
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(490, 69);
            btnOk.Margin = new Padding(4, 4, 4, 4);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(100, 28);
            btnOk.TabIndex = 998;
            btnOk.Text = @"&Ok";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel = new Button();
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(598, 69);
            btnCancel.Margin = new Padding(4, 4, 4, 4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 28);
            btnCancel.TabIndex = 999;
            btnCancel.Text = @"&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // SimpleReflectionForm
            // 
            AutoScaleMode = AutoScaleMode.Inherit;
            ClientSize = new Size(714, 112);
            AutoScaleDimensions = ClientSize;// new SizeF(8F, 16F);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Margin = new Padding(4, 4, 4, 4);
            Name = "SimpleReflectionForm";
            Text = @"SimpleReflectionForm";
            ResumeLayout(false);
        }

        #endregion
    }
}
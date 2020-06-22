//------------------------------------------------------------------------------
// <copyright file="QueuePathDialog.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace MSMQ.Messaging.Design
{
   

    /// <include file='..\..\doc\QueuePathDialog.uex' path='docs/doc[@for="QueuePathDialog"]/*' />
    /// <internalonly/>
    /// <devdoc>
    /// </devdoc>
    public class QueuePathDialog : Form
    {
        private static readonly string HELP_KEYWORD = "System.Messaging.Design.QueuePathDialog";

        private ImageList icons;

        private Button okButton;

        private ComboBox pathType;

        private TreeView enterprise;

        private Button helpButton;

        private Label selectLabel;

        private Label referenceLabel;

        private Button cancelButton;

        private string path = String.Empty;
        private string queuePath = String.Empty;
        private int lastPathType = 0;
        private bool exit;
        private MessageQueue selectedQueue;
        private delegate void FinishPopulateDelegate(MessageQueue[] queues);
        private delegate void SelectQueueDelegate(MessageQueue queue, int pathTypeIndex);
        private delegate void ShowErrorDelegate();
        private IUIService uiService;
        private IServiceProvider provider;
        private Hashtable machinesTable = new Hashtable();

        private Thread populateThread = null;
        private bool closed = false;
        private bool populateThreadRan = false;

        //Path prefixes
        private static readonly string PREFIX_LABEL = "LABEL:";
        private static readonly string PREFIX_FORMAT_NAME = "FORMATNAME:";

        /// <include file='..\..\doc\QueuePathDialog.uex' path='docs/doc[@for="QueuePathDialog.QueuePathDialog"]/*' />
        /// <devdoc>
        ///     Creates a path editor control.
        ///     This will create also a tree view control, and an ImageList with
        ///     the icons.
        /// </devdoc>                     
        public QueuePathDialog(IServiceProvider provider)
            : base()
        {
            this.uiService = (IUIService)provider.GetService(typeof(IUIService));
            this.provider = provider;
            this.InitializeComponent();
        }


        /// <include file='..\..\doc\QueuePathDialog.uex' path='docs/doc[@for="QueuePathDialog.QueuePathDialog2"]/*' />
        /// <devdoc>
        ///     Creates a path editor control.
        ///     This will create also a tree view control, and an ImageList with
        ///     the icons.
        /// </devdoc>
        public QueuePathDialog(IUIService uiService)
            : base()
        {
            this.uiService = uiService;
            this.InitializeComponent();
        }

        /// <include file='..\..\doc\QueuePathDialog.uex' path='docs/doc[@for="QueuePathDialog.Path"]/*' />
        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        public string Path
        {
            get
            {
                return this.queuePath;
            }
        }

        private void AfterSelect(object source, TreeViewEventArgs e)
        {
            TreeNode node = (TreeNode)e.Node;
            string[] nodeTexts = node.FullPath.Split(new char[] { '\\' });

            if (nodeTexts.Length == 2)
            {
                StringBuilder path = new StringBuilder();
                path.Append(node.Parent.Text);
                path.Append("\\");
                path.Append(nodeTexts[1]);
                this.path = path.ToString();
                ChoosePath();
                exit = true;
            }
        }

        private void BeforeSelect(object source, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            string[] nodeTexts = node.FullPath.Split(new char[] { '\\' });
            node.SelectedImageIndex = nodeTexts.Length - 1;
            exit = false;
        }

        /// <include file='..\..\doc\QueuePathDialog.uex' path='docs/doc[@for="QueuePathDialog.ChoosePath"]/*' />
        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        [SuppressMessage("Microsoft.Security", "CA2102:CatchNonClsCompliantExceptionsInGeneralHandlers")]
        public void ChoosePath()
        {
            if (this.path != null && this.path != String.Empty)
            {
                if (pathType.Text.CompareTo(Res.GetString(Res.RefByPath)) == 0)
                {
                    this.queuePath = this.path;
                    this.lastPathType = this.pathType.SelectedIndex;
                }
                else if (pathType.Text.CompareTo(Res.GetString(Res.RefByFormatName)) == 0)
                {
                    MessageQueue queue = new MessageQueue(this.path);
                    this.queuePath = PREFIX_FORMAT_NAME + queue.FormatName;
                    this.lastPathType = this.pathType.SelectedIndex;
                }
                else
                {
                    MessageQueue queue = new MessageQueue(this.path);
                    string tempPath = PREFIX_LABEL + queue.Label;
                    try
                    {
                        MessageQueue validate = new MessageQueue(tempPath);
                        string stringValidate = validate.FormatName;
                        this.queuePath = tempPath;
                        this.lastPathType = this.pathType.SelectedIndex;
                    }
                    // validate.FormatName can result in a deep stack, making it diffucult
                    // to enumerate all exception types that can be thrown. At the same time,
                    // we dont want to catch non-CLS exceptions to make users aware of them 
                    // (since this is design time). Hence, SuppressMessage above
                    catch (Exception e)
                    {
                        if (this.queuePath != null && String.Compare(this.queuePath, tempPath, true, CultureInfo.InvariantCulture) != 0)
                        {
                            exit = false;
                            if (uiService != null)
                                uiService.ShowError(e.Message);
                            else
                                MessageBox.Show(e.Message, String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            if (this.queuePath == String.Empty)
                            {
                                this.queuePath = this.path;
                                this.lastPathType = 0;
                            }

                            OnSelectQueue(new MessageQueue(this.queuePath), this.lastPathType);
                        }
                    }
                }
            }
        }

        /// <include file='..\..\doc\QueuePathDialog.uex' path='docs/doc[@for="QueuePathDialog.DoubleClicked"]/*' />
        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        // Supressing FxCop violations since it is breaking change
        [SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
        public void DoubleClicked(object source, EventArgs e)
        {
            if (exit)
            {
                this.Close();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void IndexChanged(object source, EventArgs e)
        {
            ChoosePath();
        }

        private void InitializeComponent()
        {
            ResourceManager resources = new ResourceManager(typeof(QueuePathDialog));
            this.icons = new ImageList();
            this.okButton = new Button();
            this.pathType = new ComboBox();
            this.enterprise = new TreeView();
            this.helpButton = new Button();
            this.selectLabel = new Label();
            this.referenceLabel = new Label();
            this.cancelButton = new Button();
            this.okButton.Location = ((Point)(resources.GetObject("okButton.Location")));
            this.okButton.Size = ((Size)(resources.GetObject("okButton.Size")));
            this.okButton.TabIndex = ((int)(resources.GetObject("okButton.TabIndex")));
            this.okButton.Text = resources.GetString("okButton.Text");
            this.okButton.DialogResult = DialogResult.OK;
            this.pathType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.pathType.DropDownWidth = 264;
            this.pathType.Items.Add(Res.GetString(Res.RefByPath));
            this.pathType.Items.Add(Res.GetString(Res.RefByFormatName));
            this.pathType.Items.Add(Res.GetString(Res.RefByLabel));
            this.pathType.SelectedIndex = 0;
            this.pathType.Location = ((Point)(resources.GetObject("pathType.Location")));
            this.pathType.Size = ((Size)(resources.GetObject("pathType.Size")));
            this.pathType.TabIndex = ((int)(resources.GetObject("pathType.TabIndex")));
            this.pathType.SelectedIndexChanged += new EventHandler(this.IndexChanged);
            this.enterprise.HideSelection = false;
            this.enterprise.ImageIndex = -1;
            this.enterprise.Location = ((Point)(resources.GetObject("enterprise.Location")));
            this.enterprise.Nodes.AddRange(new TreeNode[] { new TreeNode(Res.GetString(Res.PleaseWait)) });
            this.enterprise.SelectedImageIndex = -1;
            this.enterprise.Size = ((Size)(resources.GetObject("enterprise.Size")));
            this.enterprise.Sorted = true;
            this.enterprise.TabIndex = ((int)(resources.GetObject("enterprise.TabIndex")));
            this.enterprise.AfterSelect += new TreeViewEventHandler(this.AfterSelect);
            this.enterprise.BeforeSelect += new TreeViewCancelEventHandler(this.BeforeSelect);
            this.enterprise.DoubleClick += new EventHandler(this.DoubleClicked);
            this.enterprise.ImageList = icons;
            this.helpButton.Location = ((Point)(resources.GetObject("helpButton.Location")));
            this.helpButton.Size = ((Size)(resources.GetObject("helpButton.Size")));
            this.helpButton.TabIndex = ((int)(resources.GetObject("helpButton.TabIndex")));
            this.helpButton.Text = resources.GetString("helpButton.Text");
            this.helpButton.Click += new EventHandler(this.OnClickHelpButton);
            this.icons.Images.Add(new Bitmap(typeof(MessageQueue), "Machine.bmp"));
            this.icons.Images.Add(new Bitmap(typeof(MessageQueue), "PublicQueue.bmp"));
            this.selectLabel.Location = ((Point)(resources.GetObject("selectLabel.Location")));
            this.selectLabel.Size = ((Size)(resources.GetObject("selectLabel.Size")));
            this.selectLabel.TabIndex = ((int)(resources.GetObject("selectLabel.TabIndex")));
            this.selectLabel.Text = resources.GetString("selectLabel.Text");
            this.referenceLabel.Location = ((Point)(resources.GetObject("referenceLabel.Location")));
            this.referenceLabel.Size = ((Size)(resources.GetObject("referenceLabel.Size")));
            this.referenceLabel.TabIndex = ((int)(resources.GetObject("referenceLabel.TabIndex")));
            this.referenceLabel.Text = resources.GetString("referenceLabel.Text");
            this.cancelButton.DialogResult = DialogResult.Cancel;
            this.cancelButton.Location = ((Point)(resources.GetObject("cancelButton.Location")));
            this.cancelButton.Size = ((Size)(resources.GetObject("cancelButton.Size")));
            this.cancelButton.TabIndex = ((int)(resources.GetObject("cancelButton.TabIndex")));
            this.cancelButton.Text = resources.GetString("cancelButton.Text");
            this.cancelButton.DialogResult = DialogResult.Cancel;
            this.HelpRequested += new HelpEventHandler(this.OnHelpRequested);
            this.AcceptButton = this.okButton;
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoScaleDimensions = new SizeF(6.0F, 14.0F);
            this.CancelButton = this.cancelButton;
            this.ClientSize = ((Size)(resources.GetObject("$this.ClientSize")));
            this.Controls.AddRange(new Control[] { this.helpButton,
                        this.cancelButton,
                        this.okButton,
                        this.pathType,
                        this.referenceLabel,
                        this.enterprise,
                        this.selectLabel });
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Win32Form1";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = resources.GetString("$this.Text");
            this.lastPathType = 0;
            this.Icon = null;
        }

        // PopulateThread calls BeginInvoke in case of failure. BeginInvoke requires window handle 
        // to exist for this Form. Therefore, PopulateThread can be called safely only after handle is created
        // Call it in OnHandleCreated event handler
        private void PopulateThread()
        {
            try
            {
                IEnumerator messageQueues = MessageQueue.GetMessageQueueEnumerator();
                bool locate = true;
                while (locate)
                {
                    MessageQueue[] queues = new MessageQueue[100];
                    for (int index = 0; index < queues.Length; ++index)
                    {
                        if (messageQueues.MoveNext())
                            queues[index] = (MessageQueue)messageQueues.Current;
                        else
                        {
                            queues[index] = null;
                            locate = false;
                        }
                    }

                    this.BeginInvoke(new FinishPopulateDelegate(this.OnPopulateTreeview), new object[] { queues });
                }
            }
            catch
            {
                if (!this.closed)
                    this.BeginInvoke(new ShowErrorDelegate(this.OnShowError), null);
            }

            if (!this.closed)
                this.BeginInvoke(new SelectQueueDelegate(this.OnSelectQueue), new object[] { this.selectedQueue, 0 });
        }

        /// <include file='..\..\doc\QueuePathDialog.uex' path='docs/doc[@for="QueuePathDialog.SelectQueue"]/*' />
        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        public void SelectQueue(MessageQueue queue)
        {
            this.selectedQueue = queue;
        }

        private void OnHelpRequested(object sender, HelpEventArgs e)
        {
            OnClickHelpButton(null, null);
        }

        private void OnClickHelpButton(object source, EventArgs e)
        {
            if (this.provider != null)
            {
                IHelpService helpService = (IHelpService)provider.GetService(typeof(IHelpService));
                if (helpService != null)
                {
                    helpService.ShowHelpFromKeyword(HELP_KEYWORD);
                }
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.closed = true;

            if (populateThread != null)
            {
                populateThread.Abort();
            }

            base.OnFormClosing(e);
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            // Window handle has been created, so it's safe to call PopulateThread now.
            // However, OnHandleCreated can be called multiple times during Form's life-time.
            // So we need to ensure that PopulateThread runs only once.
            // ( eugenesh May 27 2005. Whidbey 463310 )
            if (!populateThreadRan)
            {
                populateThreadRan = true;
                populateThread = new Thread(new ThreadStart(this.PopulateThread));
                populateThread.Start();
            }

            base.OnHandleCreated(e);
        }


        private void OnPopulateTreeview(MessageQueue[] queues)
        {
            if (queues == null || queues.Length == 0)
            {
                return;
            }

            if (machinesTable.Count == 0)
                enterprise.Nodes.Clear();

            for (int index = 0; index < queues.Length; ++index)
            {
                if (queues[index] != null)
                {
                    string machineName = queues[index].MachineName;
                    TreeNode machineNode = null;
                    if (machinesTable.ContainsKey(machineName))
                        machineNode = (TreeNode)machinesTable[machineName];
                    else
                    {
                        machineNode = enterprise.Nodes.Add(machineName);
                        machinesTable[machineName] = machineNode;
                    }

                    machineNode.Nodes.Add(queues[index].QueueName).ImageIndex = 1;
                }
            }
        }


        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions")]
        private void OnShowError()
        {
            if (uiService != null)
                uiService.ShowError(Res.GetString(Res.QueueNetworkProblems));
            else
                MessageBox.Show(Res.GetString(Res.QueueNetworkProblems), String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void OnSelectQueue(MessageQueue queue, int pathTypeIndex)
        {
            try
            {
                pathType.SelectedIndex = pathTypeIndex;
                string machineName = queue.MachineName;
                string queueName = queue.QueueName;
                TreeNodeCollection machines = enterprise.Nodes;
                for (int index = 0; index < machines.Count; ++index)
                {
                    TreeNode machine = machines[index];
                    if (String.Compare(machineName, machine.Text, true, CultureInfo.InvariantCulture) == 0)
                    {
                        machine.Expand();
                        //Need to flush all events so that the children nodes get added.
                        Application.DoEvents();
                        TreeNodeCollection queues = machine.Nodes;
                        for (int index2 = 0; index2 < queues.Count; ++index2)
                        {
                            TreeNode queueNode = queues[index2];
                            if (queueNode.Text != null && String.Compare(queueName, queueNode.Text, true, CultureInfo.InvariantCulture) == 0)
                            {
                                enterprise.SelectedNode = queueNode;
                                return;
                            }
                        }
                        return;
                    }
                }
            }
            catch
            {
                //Ignore all Exceptions.
            }
        }
    }
}

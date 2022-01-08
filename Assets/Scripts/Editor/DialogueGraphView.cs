using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DialogueGraphView : GraphView
{
    private readonly Vector2 defaultNodeSize = new Vector2(100,150);
    public DialogueGraphView()
    {

        this.styleSheets.Add(Resources.Load<StyleSheet>("Dialogue"));
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);

        var grid = new GridBackground();
        Insert(0,grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode(){
            title = "Start",
            GUID = Guid.NewGuid().ToString(),
            DialogueText = "Dialogue text",
            EntryPoint = true,
        };

        var port = GeneratePort(node,Direction.Output);
        port.portName = "Next";

        node.SetPosition(new Rect(100,100,100,150));

        node.outputContainer.Add(port);

        node.RefreshExpandedState();
        node.RefreshPorts();

        return node;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var ports = new List<Port>();

        ports.ForEach((port)=>
        {
            if(startPort!=port && startPort.node!= port.node){
                ports.Add(port);
            }
        });

        return ports;
    }

    public void CreateNode(string nodeName)
    {
        AddElement(CreateDialogueNode(nodeName));
    }

    private Port GeneratePort(DialogueNode node,Direction direction, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Vertical,direction,capacity,typeof(int));
    }

    public DialogueNode CreateDialogueNode(string nodeName)
    {   
        var dialogueNode = new DialogueNode{
            title = nodeName,
            DialogueText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };
        var inputPort = GeneratePort(dialogueNode,Direction.Input,Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        var button = new Button(()=>{
            AddChoicePort(dialogueNode);
        });
        button.text = "New Choice";
        dialogueNode.titleContainer.Add(button);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(Vector2.zero,defaultNodeSize));
        return dialogueNode;
    }

    private void AddChoicePort(DialogueNode dialogueNode)
    {
        var generatePort = GeneratePort(dialogueNode,Direction.Output);

        var outPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count();
        generatePort.portName = $"Choice{outPortCount}";

        dialogueNode.outputContainer.Add(generatePort);
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }
}

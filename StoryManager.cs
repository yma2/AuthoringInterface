using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

// This class represents the crime scene narrative.
public class StoryManager {
	
	// The list of steps in the narrative.
	public List< StoryUnit > steps;
	// The next step ID to assign.
	public int nextStepID;
	
	//This is the StoryManager constructor.
	public StoryManager() {
		this.nextStepID = 0;
		this.steps = new List<StoryUnit>();
	}
	
	// This function adds a step to the narrative.
	public int addStep( string actionName, List< string > parameters ) {
		StoryUnit step = new StoryUnit( actionName, this.nextStepID );
		this.nextStepID++;
		foreach ( string parameter in parameters ) {
			step.addParameter( parameter );	
		}
		steps.Add( step );
        return nextStepID - 1;
	}

    // Overloaded add step function
    public int addStep(string name, List<string> paramNames, List<string> paramValues, List<int> tailID)
    {
        int stepID = this.addStep(name, paramNames);

        for (int i = 0; i < paramNames.Count; i++)
        {
            this.changeStepParameterValue(stepID, paramNames[i], paramValues[i]);
        }
        for (int i = 0; i < tailID.Count; i++)
        {
            this.reorderStep(stepID, tailID[i]);
        }

        return stepID;

    }
	
	// This function removes a step from the narrative.
	public void removeStep( StoryUnit step ) {
		steps.Remove( step );
	}
	
	// This function changes the value of the specified step's parameter.
	public void changeStepParameterValue( int stepID, string name, string val ) {
		foreach ( StoryUnit step in steps ) {
			if ( step.stepID == stepID ) {
				step.setParameterValue( name, val );
				break;
			}
		}
	}
	
	// This function sets the precondition step for the specified step.
	public void reorderStep( int stepID, int tailID ) {
			foreach ( StoryUnit step in steps ) {
			if ( step.stepID == stepID ) {
				step.setPreconditionStep( tailID );
				break;
			}
		}
	}
	
	// This function generates the XML for the entire narrative.
	public void generateXML() {
		// Create the XML file ( TODO: Make the path correctly relative for IC-CRIME installations, customize name somehow )
		XmlTextWriter xmlGenerator = new XmlTextWriter( "test-plan.xml", null );
		
		// Format the XML generator
		xmlGenerator.Formatting = Formatting.Indented;
		xmlGenerator.Indentation = 4;
		
		// Open/create the XML document
        xmlGenerator.WriteStartDocument();
		
		// Write the plan and steps
		xmlGenerator.WriteStartElement( "plan" );		
		xmlGenerator.WriteStartElement( "steps" );
		
		// Write each step
		foreach ( StoryUnit step in steps ) {
			step.generateStepXML( xmlGenerator );	
		}
		
		// End the steps
		xmlGenerator.WriteEndElement();
		
		// Write the orderings
		xmlGenerator.WriteStartElement( "orderings" );
		
		// Write each ordering
		foreach ( StoryUnit step in steps ) {
			step.generateOrderingXML( xmlGenerator );			
		}
		
		// End the XML document
        xmlGenerator.WriteEndDocument();
        
		// Close the XML writer
        xmlGenerator.Close();
	}
	
	#region StoryUnit class
	
	// This class represents a single action in the narrative.
	public class StoryUnit {
		
		// The name of the BOLA action this associates with.
		public string name;		
		// The unique step ID for this action in the narrative.
		public int stepID;		
		// The list of the parameters required for successful exection of the action.
		public List< KeyValuePair< string, string > > parameters;		
		// The step ID that is a precondition to this step.
		public List<int> tailID;
		
		// This is the StoryUnit constructor.
		public StoryUnit( string name, int stepID ) {
			this.name = name;
			this.stepID = stepID;
			this.parameters = new List< KeyValuePair< string, string > >();
			this.tailID = new List<int>();
			this.tailID.Add (-1);
		}
		
		// This function adds a parameter to the step's action.
		public void addParameter( string name ) {
			this.parameters.Add( new KeyValuePair< string, string >( name, null ) );
		}
		
		// This function sets the value of a given parameter.
		public void setParameterValue( string name, string val ) {
			for ( int i = 0; i < parameters.Count; i++ ) {
				if ( parameters[ i ].Key.Equals( name ) ) {
					parameters[ i ] = new KeyValuePair< string, string >( name, val );
					break;
				}
			}
		}
		
		// This function sets this step's precondition step.
		public void setPreconditionStep( int tailID ) {
			if (this.tailID[0].Equals(-1)) {
				this.tailID.Remove(-1);
			}
			this.tailID.Add(tailID);
		}
		
		// This function generates the XML for this individual step.
		public void generateStepXML( XmlTextWriter xmlGenerator ) {
			
			// Write the step id
			xmlGenerator.WriteStartElement( "step" );
			xmlGenerator.WriteAttributeString( "id", this.stepID.ToString() );
			
			// Write the name
			xmlGenerator.WriteStartElement( "name" );
			xmlGenerator.WriteString( this.name.ToUpper() );
			xmlGenerator.WriteEndElement();
			
			// Write the parameters
			xmlGenerator.WriteStartElement( "parameters" );
			
			// Write each parameter
			foreach ( KeyValuePair< string, string > parameter in parameters ) {
				xmlGenerator.WriteStartElement( "parameter" );
				xmlGenerator.WriteAttributeString( "name", parameter.Key );
				xmlGenerator.WriteString( parameter.Value );
				xmlGenerator.WriteEndElement();
			}
			
			// End the parameters and the step
			xmlGenerator.WriteEndElement();
			xmlGenerator.WriteEndElement();
		}
		
		// This function generates the XML for this step's ordering.
		public void generateOrderingXML( XmlTextWriter xmlGenerator ) {
           			
			if (tailID[0].Equals(-1)) {
				return;
			}
			
			for (int i = 0; i < tailID.Count; i++) {
			
				// Write the ordering
				xmlGenerator.WriteStartElement( "ordering" );
				
				// Write the tail
				xmlGenerator.WriteStartElement( "tail" );
				xmlGenerator.WriteString( this.tailID[i].ToString() );
				xmlGenerator.WriteEndElement();
				
				// Write the head
				xmlGenerator.WriteStartElement( "head" );
				xmlGenerator.WriteString( this.stepID.ToString() );
				xmlGenerator.WriteEndElement();
				
				// End the ordering
				xmlGenerator.WriteEndElement();
			}
		}
	}
	
	#endregion StoryUnit class
}

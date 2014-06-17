using System;

namespace MonoBrickFirmware.Movement
{
	/// <summary>
	/// Class for controlling a vehicle
	/// </summary>
	public class Vehicle{
		private MotorSync motorSync = new MotorSync(MotorPort.OutA, MotorPort.OutD);
		private MotorPort leftPort;
		private MotorPort rightPort;
		
		/// <summary>
		/// Initializes a new instance of the Vehicle class.
		/// </summary>
		/// <param name='left'>
		/// The left motor of the vehicle
		/// </param>
		/// <param name='right'>
		/// The right motor of the vehicle
		/// </param>
		public Vehicle(MotorPort left, MotorPort right){
			LeftPort = left;
			RightPort = right;
		}
	
		/// <summary>
		/// Gets or sets the left motor
		/// </summary>
		/// <value>
		/// The left motor
		/// </value>
		public MotorPort LeftPort{
			get{
				return leftPort;
			}
			set{
				leftPort = value;
				motorSync.BitField = motorSync.MotorPortToBitfield(leftPort) | motorSync.MotorPortToBitfield(rightPort);
			}
		}
	
		/// <summary>
		/// Gets or sets the right motor
		/// </summary>
		/// <value>
		/// The right motor
		/// </value>
		public MotorPort RightPort{
			get{
				return rightPort;
			}
			set{
				rightPort = value;
				motorSync.BitField = motorSync.MotorPortToBitfield(leftPort) | motorSync.MotorPortToBitfield(rightPort);
			}
		}
	
		/// <summary>
		/// Gets or sets a value indicating whether the left motor is running in reverse direction
		/// </summary>
		/// <value>
		/// <c>true</c> if left motor is reverse; otherwise, <c>false</c>.
		/// </value>
		public bool ReverseLeft{get; set;}
	
		/// <summary>
		/// Gets or sets a value indicating whether the right motor is running in reverse direction
		/// </summary>
		/// <value>
		/// <c>true</c> if right motor is reverse; otherwise, <c>false</c>.
		/// </value>
		public bool ReverseRight{get;set;}
	
		/// <summary>
		/// Run backwards
		/// </summary>
		/// <param name='speed'>
		/// Speed of the vehicle -100 to 100
		/// </param>
		public void Backward(sbyte speed){
			Backward(speed, 0, false, false);
		}
	
		/// <summary>
		/// Run backwards
		/// </summary>
		/// <param name="speed">Speed.</param>
		/// <param name="steps">Steps.</param>
		/// <param name="brake">If set to <c>true</c> motors will brake when done otherwise off.</param>
		/// <param name='waitForCompletion'>
		/// Set to <c>true</c> to wait for movement to be completed before returning
		/// </param>
		public void Backward(sbyte speed, UInt32 steps, bool brake, bool waitForCompletion = true){
			Move((sbyte)-speed, steps, brake, waitForCompletion);
		}
	
		/// <summary>
		/// Run forward
		/// </summary>
		/// <param name='speed'>
		/// Speed of the vehicle -100 to 100
		/// </param>
		public void Forward(sbyte speed){
			Forward(speed, 0, false, false);
		}
		
		/// <summary>
		/// Run forward
		/// </summary>
		/// <param name="speed">Speed.</param>
		/// <param name="steps">Steps.</param>
		/// <param name="brake">If set to <c>true</c> motors will brake when done otherwise off.</param>
		/// <param name='waitForCompletion'>
		/// Set to <c>true</c> to wait for movement to be completed before returning
		/// </param>
		public void Forward(sbyte speed, UInt32 steps, bool brake, bool waitForCompletion = true){
			Move(speed,steps, brake, waitForCompletion);
		}
	
		/// <summary>
		/// Spins the vehicle left.
		/// </summary>
		/// <param name="speed">Speed of the vehicle -100 to 100</param>
		public void SpinLeft(sbyte speed){
			SpinLeft(speed,0, false, false);
		}
		
		/// <summary>
		/// Spins the left.
		/// </summary>
		/// <param name="speed">Speed.</param>
		/// <param name="steps">Steps.</param>
		/// <param name="brake">If set to <c>true</c> motors will brake when done otherwise off.</param>
		/// <param name='waitForCompletion'>
		/// Set to <c>true</c> to wait for movement to be completed before returning
		/// </param>
		public void SpinLeft(sbyte speed, UInt32 steps, bool brake, bool waitForCompletion = true){
			if(leftPort < rightPort){
				HandleSpinLeft(speed, steps,  brake, waitForCompletion);	
			}
			else{
				HandleSpinRight(speed, steps, brake, waitForCompletion);	
			}
		}
		
		/// <summary>
		/// Spins the vehicle right
		/// </summary>
		/// <param name='speed'>
		/// Speed -100 to 100
		/// </param>
		public void SpinRight(sbyte speed){
			SpinRight(speed,0, false, false );
		}
		
		/// <summary>
		/// Spins the vehicle right
		/// </summary>
		/// <param name="speed">Speed.</param>
		/// <param name="steps">Steps.</param>
		/// <param name="brake">If set to <c>true</c> motors will brake when done otherwise off.</param>
		/// <param name='waitForCompletion'>
		/// Set to <c>true</c> to wait for movement to be completed before returning
		/// </param>
		public void SpinRight(sbyte speed, UInt32 steps, bool brake, bool waitForCompletion = true){
			if(leftPort < rightPort){
				HandleSpinRight(speed, steps, brake, waitForCompletion);	
			}
			else{
				HandleSpinLeft(speed, steps, brake, waitForCompletion);	
			}
		}
	
		/// <summary>
		/// Stop moving the vehicle
		/// </summary>
		public void Off(){
			motorSync.Off();
		}
	
		/// <summary>
		/// Brake the vehicle (the motor is still on but it does not move)
		/// </summary>
		public void Brake(){
			motorSync.Brake();
		}
	
		/// <summary>
		/// Turns the vehicle right
		/// </summary>
		/// <param name='speed'>
		/// Speed of the vehicle -100 to 100
		/// </param>
		/// <param name='turnPercent'>
		/// Turn percent 
		/// </param>
		public void TurnRightForward(sbyte speed, sbyte turnPercent){
			TurnRightForward(speed, turnPercent,0,false,false);
		}
		
		
		/// <summary>
		/// Turns the vehicle right
		/// </summary>
		/// <param name="speed">Speed.</param>
		/// <param name="turnPercent">Turn percent.</param>
		/// <param name="steps">Steps.</param>
		/// <param name="brake">If set to <c>true</c> motors will brake when done otherwise off.</param>
		/// <param name='waitForCompletion'>
		/// Set to <c>true</c> to wait for movement to be completed before returning
		/// </param>
		public void TurnRightForward(sbyte speed, sbyte turnPercent, UInt32 steps, bool brake, bool waitForCompletion = true){
			if(leftPort < rightPort){
				HandleRightForward(speed, turnPercent, steps, brake, waitForCompletion);	
			}
			else{
				HandleLeftForward(speed,turnPercent, steps, brake, waitForCompletion);	
			}
		}
	
		/// <summary>
		/// Turns the vehicle right while moving backwards
		/// </summary>
		/// <param name='speed'>
		/// Speed of the vehicle -100 to 100
		/// </param>
		/// <param name='turnPercent'>
		/// Turn percent.
		/// </param>
		public void TurnRightReverse(sbyte speed, sbyte turnPercent){
			TurnRightReverse(speed,turnPercent,0, false, false);
		}
		
		/// <summary>
		/// Turns the vehicle right while moving backwards
		/// </summary>
		/// <param name="speed">Speed.</param>
		/// <param name="turnPercent">Turn percent.</param>
		/// <param name="steps">Steps.</param>
		/// <param name="brake">If set to <c>true</c> motors will brake when done otherwise off.</param>
		/// <param name='waitForCompletion'>
		/// Set to <c>true</c> to wait for movement to be completed before returning
		/// </param>
		public void TurnRightReverse(sbyte speed, sbyte turnPercent, UInt32 steps, bool brake, bool waitForCompletion = true){
			if(leftPort < rightPort){
				HandleRightReverse(speed, turnPercent, steps, brake, waitForCompletion);	
			}
			else{
				HandleLeftReverse(speed,turnPercent, steps, brake, waitForCompletion);	
			}
		}
	
		/// <summary>
		/// Turns the vehicle left
		/// </summary>
		/// <param name='speed'>
		/// Speed of the vehicle -100 to 100
		/// </param>
		/// <param name='turnPercent'>
		/// Turn percent.
		/// </param>
		public void TurnLeftForward(sbyte speed, sbyte turnPercent){
			TurnLeftForward(speed,turnPercent, 0, false, false);
		}
		
		
		/// <summary>
		/// Turns the vehicle left
		/// </summary>
		/// <param name="speed">Speed.</param>
		/// <param name="turnPercent">Turn percent.</param>
		/// <param name="steps">Steps.</param>
		/// <param name="brake">If set to <c>true</c> motors will brake when done otherwise off.</param>
		/// <param name='waitForCompletion'>
		/// Set to <c>true</c> to wait for movement to be completed before returning
		/// </param>
		public void TurnLeftForward(sbyte speed, sbyte turnPercent, UInt32 steps, bool brake, bool waitForCompletion = true){
			if(leftPort < rightPort){
				HandleLeftForward(speed, turnPercent, steps, brake, waitForCompletion);	
			}
			else{
				HandleRightForward(speed,turnPercent, steps, brake, waitForCompletion);
			}
		}
	
		/// <summary>
		/// Turns the vehicle left while moving backwards
		/// </summary>
		/// <param name='speed'>
		/// Speed of the vehicle -100 to 100
		/// </param>
		/// <param name='turnPercent'>
		/// Turn percent.
		/// </param>
		public void TurnLeftReverse(sbyte speed, sbyte turnPercent){
			TurnLeftReverse(speed,turnPercent, 0, false, false);
		}
		
		
		/// <summary>
		/// Turns the vehicle left while moving backwards
		/// </summary>
		/// <param name="speed">Speed.</param>
		/// <param name="turnPercent">Turn percent.</param>
		/// <param name="steps">Steps.</param>
		/// <param name="brake">If set to <c>true</c> motors will brake when done otherwise off.</param>
		/// <param name='waitForCompletion'>
		/// Set to <c>true</c> to wait for movement to be completed before returning
		/// </param>
		public void TurnLeftReverse(sbyte speed, sbyte turnPercent, UInt32 steps, bool brake, bool waitForCompletion = true){
			if(leftPort < rightPort){
				HandleLeftReverse(speed, turnPercent, steps, brake, waitForCompletion);	
			}
			else{
				HandleRightReverse(speed,turnPercent, steps, brake, waitForCompletion);
			}
		}
		
		private void HandleLeftForward(sbyte speed, sbyte turnPercent, UInt32 steps, bool brake, bool waitForCompletion){
			if(!ReverseLeft && !ReverseRight){
				motorSync.On(speed, (short) -turnPercent, steps, brake, waitForCompletion);
			}
			if(!ReverseLeft && ReverseRight){
				motorSync.On((sbyte)-speed, (short) ((short)-200+ (short)turnPercent), steps, brake, waitForCompletion);
			}
			if(ReverseLeft && !ReverseRight){
				motorSync.On(speed, (short) ((short)-200+(short)turnPercent), steps, brake, waitForCompletion);
			}
			if(ReverseLeft && ReverseRight){
				motorSync.On((sbyte)-speed, (short) -turnPercent, steps, brake, waitForCompletion);				
			}
		}
		
		private void HandleRightForward(sbyte speed, sbyte turnPercent, UInt32 steps, bool brake, bool waitForCompletion){
			if(!ReverseLeft && !ReverseRight){
				motorSync.On(speed, (short) turnPercent, steps, brake, waitForCompletion);
			}
			if(!ReverseLeft && ReverseRight){
				motorSync.On(speed, (short) ((short)200- (short)turnPercent), steps, brake, waitForCompletion);
			}
			if(ReverseLeft && !ReverseRight){
				motorSync.On((sbyte)-speed, (short) ((short)200-(short)turnPercent), steps, brake, waitForCompletion);
			}
			if(ReverseLeft && ReverseRight){
				motorSync.On((sbyte)-speed, (short) turnPercent, steps, brake, waitForCompletion);				
			}
		}
		
		private void HandleLeftReverse (sbyte speed, sbyte turnPercent, UInt32 steps, bool brake, bool waitForCompletion)
		{
			if(!ReverseLeft && !ReverseRight){
				motorSync.On((sbyte)-speed, (short) -turnPercent, steps, brake, waitForCompletion);
			}
			if(!ReverseLeft && ReverseRight){
				motorSync.On((sbyte)speed,(short) ( (short)-200+(short)turnPercent), steps, brake, waitForCompletion);
			}
			if(ReverseLeft && !ReverseRight){
				motorSync.On((sbyte)-speed, (short) ( (short)-200+(short)turnPercent), steps, brake, waitForCompletion);
			}
			if(ReverseLeft && ReverseRight){
				motorSync.On(speed, (short) -turnPercent, steps, brake, waitForCompletion);				
			}
		}
		
		private void HandleRightReverse(sbyte speed, sbyte turnPercent, UInt32 steps, bool brake, bool waitForCompletion){
			if(!ReverseLeft && !ReverseRight){
				motorSync.On((sbyte)-speed, (short) turnPercent, steps, brake, waitForCompletion);
			}
			if(!ReverseLeft && ReverseRight){
				motorSync.On((sbyte)-speed,(short) ( (short)200-(short)turnPercent), steps, brake, waitForCompletion);
			}
			if(ReverseLeft && !ReverseRight){
				motorSync.On((sbyte)speed, (short) ( (short)200-(short)turnPercent), steps, brake, waitForCompletion);
			}
			if(ReverseLeft && ReverseRight){
				motorSync.On(speed, (short) turnPercent, steps, brake, waitForCompletion);				
			}
	
		}
		
		private void HandleSpinRight(sbyte speed, UInt32 steps, bool brake, bool waitForCompletion){
			if(!ReverseLeft && !ReverseRight){
				motorSync.On(speed, 200, steps, brake, waitForCompletion);
			}
			if(!ReverseLeft && ReverseRight){
				motorSync.On(speed, (short) 0, steps, brake, waitForCompletion);
			}
			if(ReverseLeft && !ReverseRight){
				motorSync.On((sbyte)-speed, 0, steps, brake, waitForCompletion);
			}
			if(ReverseLeft && ReverseRight){
				motorSync.On((sbyte)-speed, 200, steps, brake, waitForCompletion);				
			}
		}
		
		private void HandleSpinLeft(sbyte speed, UInt32 steps, bool brake, bool waitForCompletion){
			if(!ReverseLeft && !ReverseRight){
				motorSync.On(speed, -200, steps, brake, waitForCompletion);
			}
			if(!ReverseLeft && ReverseRight){
				motorSync.On((sbyte)-speed, (short) 0, steps, brake, waitForCompletion);
			}
			if(ReverseLeft && !ReverseRight){
				motorSync.On(speed, 0, steps, brake, waitForCompletion);
			}
			if(ReverseLeft && ReverseRight){
				motorSync.On((sbyte)-speed, -200, steps, brake, waitForCompletion);				
			}
		}
		
		private void Move(sbyte speed, UInt32 steps, bool brake, bool waitForCompletion){
			if(!ReverseLeft && !ReverseRight){
				motorSync.On(speed, 0, steps, brake, waitForCompletion); 
			}
			if(!ReverseLeft && ReverseRight){
				motorSync.On(speed, (short) 200, steps, brake, waitForCompletion);
			}
			if(ReverseLeft && !ReverseRight){
				motorSync.On(speed, -200, steps, brake, waitForCompletion);
			}
			if(ReverseLeft && ReverseRight){
				motorSync.On((sbyte)-speed, 0, steps, brake, waitForCompletion);				
			}
		}
	}
}


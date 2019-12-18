// MultiStepper.pde
// -*- mode: C++ -*-
//
// Shows how to multiple simultaneous steppers
// Runs one stepper forwards and backwards, accelerating and decelerating
// at the limits. Runs other steppers at the same time
//
// Copyright (C) 2009 Mike McCauley
// $Id: MultiStepper.pde,v 1.1 2011/01/05 01:51:01 mikem Exp mikem $

#include <AccelStepper.h>
#include <MultiStepper.h>
// Define some steppers and the pins the will use
AccelStepper stepper0(AccelStepper::FULL4WIRE, 2,4,3,5); // Defaults to AccelStepper::FULL4WIRE (4 pins) on 2, 4,3, 5
AccelStepper stepper1(AccelStepper::FULL4WIRE, 6,8,7,9);
AccelStepper stepper2(AccelStepper::FULL4WIRE, 10,12,11,13);


bool set = false;
int count = 0;
long rotations[] = {0,0,0};
int numSteppers = sizeof(rotations)/sizeof(long);

void setup()
{  
  stepper0.setMaxSpeed(500.0);
  stepper0.setAcceleration(10000.0);
  
  stepper1.setMaxSpeed(500.0);
  stepper1.setAcceleration(10000.0);

  stepper2.setMaxSpeed(500.0);
  stepper2.setAcceleration(10000.0);
  
  Serial.begin(500000);
}

void loop()
{
  //use 0-2048 for 0-360
  if(Serial.available()>2){
    rotations[0] = Serial.parseInt();
//    Serial.read();
    rotations[1] = Serial.parseInt();
//    Serial.read();
    rotations[2] = Serial.parseInt();
    
    set = true;
  }
  if(set){
    stepper0.moveTo(rotations[0]);
    stepper1.moveTo(rotations[1]);
    stepper2.moveTo(rotations[2]);
    set = false;
  }

  stepper0.run();
  stepper1.run();
  stepper2.run();
}

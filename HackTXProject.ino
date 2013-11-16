/**
*
*  0 = Index Up & Middle UP
*  1 = Index Down
*  2 = Middle Down
*
**/

#include "I2Cdev.h"
#include "MPU6050.h"

#if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE
    #include "Wire.h"
#endif

MPU6050 accelgyro;

int16_t ax, ay, az;
int16_t gx, gy, gz;

int almightyCounter = 0;
int average = 0;
int averageY = 0;
boolean firstTime = true;

// the setup routine runs once when you press reset:
void setup() {
  
  #if I2CDEV_IMPLEMENTATION == I2CDEV_ARDUINO_WIRE // initialize serial communication at 9600 bits per second:
      Wire.begin();
  #elif I2CDEV_IMPLEMENTATION == I2CDEV_BUILTIN_FASTWIRE
      Fastwire::setup(400, true);
  #endif
  
  Serial.begin(9600);
  
  accelgyro.initialize();
  accelgyro.setXAccelOffset(-76);
  if(!accelgyro.testConnection())
  {
      while(true);
  }
}


// the loop routine runs over and over again forever:
void loop() {
  
  //accelgyro.getAcceleration(&ax, &ay, &az);
  // read the input on analog pin 0:
  int middleFinger = analogRead(A0);
  int indexFinger = analogRead(A1);
  
  // print out the value you read:
  //Serial.print("Middle Finger: ");
  if((indexFinger < 525) && (middleFinger >= 500))
  {
     //Serial.println("C1");
  } else if((indexFinger >= 525) && (middleFinger >= 500))
  {
     //Serial.println("C0");
  } else if((indexFinger >= 525) && (middleFinger < 500))
  {
   //Serial.println("C2"); 
  }
  
  if(firstTime)
{
//int average = 0;
/*  
for(int i = 0; i < 10; i++)
  {
      accelgyro.getAcceleration(&ax, &ay, &az);
      if(ax > 0)
      {
        average = ax;
        //Serial.println(String(ax));
      } else {
        i--;
      }
      
  }*/
  
  
  accelgyro.getAcceleration(&ax, &ay, &az);
  average = ax;
  averageY = az;
  
  //average /= 10;
  
  //Serial.println(String(average));
  
  firstTime = false;
} else {
  almightyCounter++;
  accelgyro.getAcceleration(&ax, &ay, &az);
  
  ax = (ax - average);
  
  az = (az - averageY);
  
  if(ax < -350)
  {
   //Serial.println("L" + String(ax)); 
  } else if(ax > 350) 
  {
    //Serial.println("R" + String(ax));
  }  else {
     //Serial.println("S" );
  }
  
  if(az < -200)
  {
   Serial.println("U" + String(ay)); 
  } else if(az > 200)
  {
   Serial.println("D" + String(ay)); 
  } else {
   Serial.println("S"); 
  }
  
  if(almightyCounter == 10)
  {
   //firstTime = true; 
   //average = 0;
   //almightyCounter = 0;
  }
    
}
    
  
  //Serial.println("AX: " + String(ax) + "\tAY: " + String(ay) + "\tAZ: " + String(az));
  
  //double magnitude = sqrt(pow(ax, 2) + pow(ay, 2) + pow(az, 2));
  
  //Serial.println("Magnitude: " + String(magnitude));
  
  /*
  Serial.print("Middle Finger: ");
  Serial.println(middleFinger);
  Serial.print("Index Finger: ");
  Serial.println(indexFinger);
  */
  delay(100);        // delay in between reads for stability
}

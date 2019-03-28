#include "AX12A.h" 

#define DirectionPin 	(2u)
#define BaudRate  		(1000000ul)
#define ID				(30u)

int initial_pos = 512;
int max = initial_pos + 100;
int min = initial_pos - 100;

int pos = initial_pos;
int delta = 5;

void setup()
{
	Serial.begin(115200);
	ax12a.begin(BaudRate, DirectionPin, &Serial1);
}

void loop()
{
	int position[13] = { -1, -1, -1, 512, -1, -1, 512 ,-1, -1, -1, -1, -1, -1 };

	ax12a.moveSync(position, 13);

	delay(5000);
}

#include "PoseController.h"

PoseController::PoseController(ServoController &servoController)
	: _servoController(servoController)
{
}

void PoseController::setPoseCompleteCallback(void(*poseCompleteCallback)(int))
{
	_poseCompleteCallback = poseCompleteCallback;
}

void PoseController::queuePose(Pose &pose)
{
	_poseBuffer.add(pose);
}

void PoseController::loop()
{
	if (_servoController.isMoving()) // wait until all servos finish there current move
	{
		return;
	}
	else if (_isMoving) // servos stopped, but pose is still in moving state, end it
	{
		_isMoving = false;
		if(_poseCompleteCallback != NULL)
			_poseCompleteCallback(_currentPoseId);
	}
		
	if (_poseBuffer.isEmpty()) // see if we have another pose to play
		return;

	// play next pose
	struct Pose pose;
	if (_poseBuffer.pull(&pose))
	{
		_currentPoseId = pose.id;
		_isMoving = true;
		for (int i = 0; i < _servoController.servoCount(); i++)
		{
			if (pose.joint[i].angle >= 0)
			{
				_servoController.move(i, pose.joint[i].angle, pose.joint[i].duration);
			}
		}
		delete pose.joint;
	}
}
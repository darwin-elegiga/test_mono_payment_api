package templates

#HorizontalPodAutoscaler: {
	apiVersion: "autoscaling/v2"
	kind:       "HorizontalPodAutoscaler"
	metadata: {
		name: string
		namespace: string
		labels: _
	}
	spec: {
		scaleTargetRef: {
			apiVersion: string
			kind:       "Deployment"
			name:       string
		}
		minReplicas: int
		maxReplicas: int
		metrics: [..._]
	}
}
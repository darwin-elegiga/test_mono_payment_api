package templates

#Deployment: {
	apiVersion: "apps/v1"
	kind:       "Deployment"
	metadata: {
		name:      string
		namespace?: string
		labels?: {
			[string]: string
		}
		annotations?: {
			[string]: string
		}
	}
	spec: {
		revisionHistoryLimit?: int
		replicas: int
		strategy?: {
			type: string
			rollingUpdate?: {
				maxSurge: string
				maxUnavailable: string
			}
		}
		selector: matchLabels: {
			[string]: string
		}
		template: {
			metadata: {
				labels?: {
					[string]: string
				}
				annotations?: {
					[string]: string
				}
			}
			spec: {
				serviceAccountName?: string
				affinity?: _
				initContainers?: [...#InitContainer]
				containers: [...{
					name:            string
					image:           string
					imagePullPolicy: string
					ports?: [...{
						name:          string
						containerPort: int
					}]
								readinessProbe?: {
									httpGet: {
										path: string
										port: int
									}
								}
								livenessProbe?: {
									httpGet: {
										path: string
										port: int
									}
								}
					env?: [...{
						name:  string
						value?: string
						valueFrom?: {
							fieldRef?: {
								fieldPath: string
							}
						}
					}]
					volumeMounts?: [...{
						name:      string
						mountPath: string
						subPath?:   string
						readOnly?:  bool
					}]
					resources?: {
						requests?: {
							cpu:    string
							memory: string
						}
						limits?: {
							cpu:    string
							memory: string
						}
					}
				}]
				volumes?: [...{
					#Volume
				}]
			}
		}
	}
}
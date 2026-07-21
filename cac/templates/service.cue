package templates

#Service: {
	apiVersion: "v1"
	kind:       "Service"
	metadata: {
		name:      string
		namespace: string
		labels?: {
			[string]: string
		}
	}
	spec: {
		type: string
		selector: {
			app: string
		}
		ports: [...{
			name:       string
			port:       int
			targetPort: int
		}]
	}
}